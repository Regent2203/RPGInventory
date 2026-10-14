using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Common.Interfaces;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Application.Crafting;

public class CraftService
{
    private readonly IApplicationDbContext _context;

    public CraftService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CraftItemResponse> CraftAsync(CraftItemRequest request, CancellationToken cancellationToken = default)
    {
        var recipe = await _context.Recipes
            .Include(r => r.ResultItem)
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.IngredientItem)
            .FirstOrDefaultAsync(r => r.Id == request.RecipeId, cancellationToken);

        if (recipe == null)
            throw new ArgumentException("Recipe not found.");

        var player = await _context.Players
            .Include(p => p.InventoryItems)
            .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);

        if (player == null)
            throw new ArgumentException("Player not found.");

        foreach (var ingredient in recipe.Ingredients)
        {
            var playerItem = player.InventoryItems
                .FirstOrDefault(ii => ii.ItemId == ingredient.IngredientItemId);

            if (playerItem == null || playerItem.Quantity < ingredient.Quantity)
            {
                var itemName = ingredient.IngredientItem?.Name ?? string.Empty;
                throw new InvalidOperationException($"Not enough items for craft: {itemName}. Need: {ingredient.Quantity}.");
            }
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            var playerItem = player.InventoryItems
                .First(ii => ii.ItemId == ingredient.IngredientItemId);

            playerItem.Quantity -= ingredient.Quantity;

            if (playerItem.Quantity == 0)
            {
                _context.InventoryItems.Remove(playerItem);
            }
        }

        var craftedItemInInventory = player.InventoryItems
            .FirstOrDefault(ii => ii.ItemId == recipe.ResultItemId);

        if (craftedItemInInventory != null)
        {
            craftedItemInInventory.Quantity += recipe.ResultQuantity;
        }
        else
        {
            var newInventoryItem = new InventoryItem
            {
                Id = Guid.NewGuid(),
                PlayerId = player.Id,
                ItemId = recipe.ResultItemId,
                Quantity = recipe.ResultQuantity
            };
            _context.InventoryItems.Add(newInventoryItem);
        }

        var historyLogs = new List<InventoryHistory>();

        foreach (var ingredient in recipe.Ingredients)
        {
            historyLogs.Add(new InventoryHistory
            {
                PlayerId = request.PlayerId,
                ItemId = ingredient.IngredientItemId,
                QuantityChanged = -ingredient.Quantity,
                OperationType = InventoryOperationType.CraftConsume,
                Timestamp = DateTime.UtcNow
            });
        }
        historyLogs.Add(new InventoryHistory
        {
            PlayerId = request.PlayerId,
            ItemId = recipe.ResultItemId,
            QuantityChanged = recipe.ResultQuantity,
            OperationType = InventoryOperationType.CraftProduce,
            Timestamp = DateTime.UtcNow
        });
        _context.InventoryHistories.AddRange(historyLogs);

        await _context.SaveChangesAsync(cancellationToken);

        var updatedInventory = await _context.InventoryItems
            .Where(ii => ii.PlayerId == player.Id)
            .Include(ii => ii.Item)
            .Select(ii => new InventoryItemDto
            {
                ItemId = ii.ItemId,
                ItemName = ii.Item!.Name,
                Quantity = ii.Quantity
            })
            .ToListAsync(cancellationToken);

        return new CraftItemResponse
        {
            Message = "Craft successful.",
            CraftedItemName = recipe.ResultItem!.Name,
            CraftedQuantity = recipe.ResultQuantity,
            UpdatedInventory = updatedInventory
        };
    }
}