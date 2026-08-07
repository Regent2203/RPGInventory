using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Common.Interfaces;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Application.Shop;

public class ShopService
{
    private readonly IApplicationDbContext _context;
    private const int GoldItemId = 1; //const from DB seed

    public ShopService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TradeResponse> BuyItemAsync(TradeRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Quantity must be more than zero.");

        var item = await _context.Items.FindAsync(new object[] { request.ItemId }, cancellationToken);
        if (item == null)
            throw new ArgumentException("Item not found.");

        if (item.Id == GoldItemId)
            throw new InvalidOperationException("Cannot buy gold for gold.");

        var playerInventory = await _context.InventoryItems
            .Where(ii => ii.PlayerId == request.PlayerId)
            .ToListAsync(cancellationToken);

        var goldItem = playerInventory.FirstOrDefault(ii => ii.ItemId == GoldItemId);
        var currentGold = goldItem?.Quantity ?? 0;
        var totalCost = item.BasePrice * request.Quantity;

        if (currentGold < totalCost)
            throw new InvalidOperationException($"Not enough gold. Required: {totalCost}, currently: {currentGold}.");

        goldItem!.Quantity -= totalCost;
        if (goldItem.Quantity == 0) 
            _context.InventoryItems.Remove(goldItem);

        var inventoryItem = playerInventory.FirstOrDefault(ii => ii.ItemId == item.Id);
        if (inventoryItem != null)
        {
            inventoryItem.Quantity += request.Quantity;
        }
        else
        {
            inventoryItem = new InventoryItem
            {
                Id = Guid.NewGuid(),
                PlayerId = request.PlayerId,
                ItemId = item.Id,
                Quantity = request.Quantity
            };
            _context.InventoryItems.Add(inventoryItem);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new TradeResponse
        {
            Message = $"Buy successful. Item: {item.Name}, quantity: {request.Quantity}",
            GoldBalance = goldItem.Quantity,
            ItemInventoryQuantity = inventoryItem.Quantity
        };
    }

    // 💰 ПРОДАЖА ПРЕДМЕТА ТОРГОВЦУ
    public async Task<TradeResponse> SellItemAsync(TradeRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
            throw new ArgumentException("Quantity must be more than zero.");

        if (request.ItemId == GoldItemId)
            throw new InvalidOperationException("Cannot sell gold for gold.");

        var playerInventory = await _context.InventoryItems
            .Where(ii => ii.PlayerId == request.PlayerId)
            .Include(ii => ii.Item)
            .ToListAsync(cancellationToken);

        var inventoryItem = playerInventory.FirstOrDefault(ii => ii.ItemId == request.ItemId);

        if (inventoryItem == null || inventoryItem.Quantity < request.Quantity)
        {
            throw new InvalidOperationException("Not enough items for sell.");
        }

        inventoryItem.Quantity -= request.Quantity;
        var remainingItems = inventoryItem.Quantity;

        if (inventoryItem.Quantity == 0)
        {
            _context.InventoryItems.Remove(inventoryItem);
        }

        var totalPayout = inventoryItem.Item!.BasePrice * request.Quantity;
        var goldItem = playerInventory.FirstOrDefault(ii => ii.ItemId == GoldItemId);

        if (goldItem != null)
        {
            goldItem.Quantity += totalPayout;
        }
        else
        {
            goldItem = new InventoryItem
            {
                Id = Guid.NewGuid(),
                PlayerId = request.PlayerId,
                ItemId = GoldItemId,
                Quantity = totalPayout
            };
            _context.InventoryItems.Add(goldItem);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new TradeResponse
        {
            Message = $"Sell successful. Item: {inventoryItem.Item.Name}, quantity: {request.Quantity}",
            GoldBalance = goldItem.Quantity,
            ItemInventoryQuantity = remainingItems
        };
    }
}