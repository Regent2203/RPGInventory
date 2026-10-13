using RPGInventory.Domain.Definitions;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Domain.Constants;

public static class GamePrices
{
    public static readonly IReadOnlyList<ItemPrice> All = new List<ItemPrice>
    {
        new() { TargetItemId = GameItems.WoodId, CurrencyItemId = GameItems.GoldId, Quantity = 5 },
        
        new() { TargetItemId = GameItems.IronOreId, CurrencyItemId = GameItems.GoldId, Quantity = 10 },
        
        new() { TargetItemId = GameItems.IronIngotId, CurrencyItemId = GameItems.GoldId, Quantity = 25 },
        
        new() { TargetItemId = GameItems.SteelSwordId, CurrencyItemId = GameItems.GoldId, Quantity = 50 },
        new() { TargetItemId = GameItems.SteelSwordId, CurrencyItemId = GameItems.IronIngotId, Quantity = 2 }
    };
}
