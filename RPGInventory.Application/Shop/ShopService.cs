using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Common.Interfaces;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Application.Shop;

public class ShopService
{
    private readonly IApplicationDbContext _context;

    public ShopService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TradeResponse> BuyItemAsync(TradeRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Quantity must be more than zero.");

        var itemToBuy = await _context.Items
            .Include(i => i.Prices)
            .ThenInclude(p => p.CurrencyItem)
            .FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

        if (itemToBuy == null)
            throw new ArgumentException("Item not found.");

        if (itemToBuy.Prices.Count == 0)
            throw new InvalidOperationException("This item cannot be bought (price doesn't exist).");

        var playerInventory = await _context.InventoryItems
            .Where(ii => ii.PlayerId == request.PlayerId)
            .Include(ii => ii.Item)
            .ToListAsync(cancellationToken);

        foreach (var price in itemToBuy.Prices)
        {
            int totalRequired = price.Quantity * request.Quantity;
            var playerCurrency = playerInventory.FirstOrDefault(ii => ii.ItemId == price.CurrencyItemId);
            var currentAmount = playerCurrency?.Quantity ?? 0;

            if (currentAmount < totalRequired)
            {
                var currencyName = price.CurrencyItem?.Name ?? $"[ItemId = {price.CurrencyItemId}]";
                throw new InvalidOperationException($"Not enough {currencyName} for buying {itemToBuy.Name}. Required: {totalRequired}, currently: {currentAmount}.");
            }
        }

        var historyLogs = new List<InventoryHistory>();
        var updatedItemsDto = new List<UpdatedItemDto>();

        foreach (var price in itemToBuy.Prices)
        {
            int totalRequired = price.Quantity * request.Quantity;
            var playerCurrency = playerInventory.First(ii => ii.ItemId == price.CurrencyItemId);

            playerCurrency.Quantity -= totalRequired;

            historyLogs.Add(new InventoryHistory
            {
                PlayerId = request.PlayerId,
                ItemId = price.CurrencyItemId,
                QuantityChanged = -totalRequired,
                OperationType = InventoryOperationType.Purchase,
                Timestamp = DateTime.UtcNow
            });

            updatedItemsDto.Add(new UpdatedItemDto
            {
                ItemId = price.CurrencyItemId,
                ItemName = playerCurrency.Item?.Name ?? $"[ItemId = {price.CurrencyItemId}]",
                NewQuantity = playerCurrency.Quantity
            });

            if (playerCurrency.Quantity == 0)
            {
                _context.InventoryItems.Remove(playerCurrency);
            }
        }

        var itemToBuyInInventory = playerInventory.FirstOrDefault(ii => ii.ItemId == itemToBuy.Id);
        if (itemToBuyInInventory != null)
        {
            itemToBuyInInventory.Quantity += request.Quantity;
        }
        else
        {
            itemToBuyInInventory = new InventoryItem
            {
                Id = Guid.NewGuid(),
                PlayerId = request.PlayerId,
                ItemId = itemToBuy.Id,
                Quantity = request.Quantity
            };
            _context.InventoryItems.Add(itemToBuyInInventory);
        }

        historyLogs.Add(new InventoryHistory
        {
            PlayerId = request.PlayerId,
            ItemId = itemToBuy.Id,
            QuantityChanged = request.Quantity,
            OperationType = InventoryOperationType.Purchase,
            Timestamp = DateTime.UtcNow
        });

        _context.InventoryHistories.AddRange(historyLogs);
        await _context.SaveChangesAsync(cancellationToken);

        updatedItemsDto.Add(new UpdatedItemDto
        {
            ItemId = itemToBuy.Id,
            ItemName = itemToBuy.Name,
            NewQuantity = itemToBuyInInventory.Quantity
        });

        return new TradeResponse
        {
            Message = $"You have successfully bought {request.Quantity} {itemToBuy.Name}.",
            UpdatedItems = updatedItemsDto
        };
    }

    public async Task<TradeResponse> SellItemAsync(TradeRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Quantity must be more than zero.");

        var playerInventory = await _context.InventoryItems
            .Where(ii => ii.PlayerId == request.PlayerId)
            .Include(ii => ii.Item)
            .ToListAsync(cancellationToken);

        var itemToSellInInventory = playerInventory.FirstOrDefault(ii => ii.ItemId == request.ItemId);
        if (itemToSellInInventory == null || itemToSellInInventory.Quantity < request.Quantity)
        {
            throw new InvalidOperationException($"Not enough items in inventory for selling.");
        }

        var itemToSell = await _context.Items
            .Include(i => i.Prices)
            .ThenInclude(p => p.CurrencyItem)
            .FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

        if (itemToSell == null || itemToSell.Prices.Count == 0)
        {
            throw new InvalidOperationException("This item cannot be sold (price doesn't exist).");
        }

        var historyLogs = new List<InventoryHistory>();
        var updatedItemsDto = new List<UpdatedItemDto>();

        itemToSellInInventory.Quantity -= request.Quantity;
        int remainingItemsCount = itemToSellInInventory.Quantity;

        updatedItemsDto.Add(new UpdatedItemDto
        {
            ItemId = itemToSell.Id,
            ItemName = itemToSell.Name,
            NewQuantity = remainingItemsCount
        });

        historyLogs.Add(new InventoryHistory
        {
            PlayerId = request.PlayerId,
            ItemId = itemToSell.Id,
            QuantityChanged = -request.Quantity,
            OperationType = InventoryOperationType.Sale,
            Timestamp = DateTime.UtcNow
        });

        if (itemToSellInInventory.Quantity == 0)
        {
            _context.InventoryItems.Remove(itemToSellInInventory);
        }

        foreach (var price in itemToSell.Prices)
        {
            int totalPayout = price.Quantity * request.Quantity;

            var currencyInInventory = playerInventory.FirstOrDefault(ii => ii.ItemId == price.CurrencyItemId);

            if (currencyInInventory != null)
            {
                currencyInInventory.Quantity += totalPayout;
            }
            else
            {
                currencyInInventory = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    PlayerId = request.PlayerId,
                    ItemId = price.CurrencyItemId,
                    Quantity = totalPayout
                };
                _context.InventoryItems.Add(currencyInInventory);

                //currencyInInventory.Item = price.CurrencyItem;
            }

            historyLogs.Add(new InventoryHistory
            {
                PlayerId = request.PlayerId,
                ItemId = price.CurrencyItemId,
                QuantityChanged = totalPayout,
                OperationType = InventoryOperationType.Sale,
                Timestamp = DateTime.UtcNow
            });

            updatedItemsDto.Add(new UpdatedItemDto
            {
                ItemId = price.CurrencyItemId,
                ItemName = price.CurrencyItem?.Name ?? $"[ItemId = {price.CurrencyItemId}]",
                NewQuantity = currencyInInventory.Quantity
            });
        }

        _context.InventoryHistories.AddRange(historyLogs);
        await _context.SaveChangesAsync(cancellationToken);

        return new TradeResponse
        {
            Message = $"You have successfully sold {request.Quantity} {itemToSell.Name}.",
            UpdatedItems = updatedItemsDto
        };
    }

}