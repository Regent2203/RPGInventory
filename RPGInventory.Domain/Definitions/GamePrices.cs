using RPGInventory.Domain.Entities;

namespace RPGInventory.Domain.Definitions;

public static class GamePrices
{
    public static readonly IReadOnlyList<ItemPrice> All = new List<ItemPrice>
    {
        //resource
        new() { TargetItemId = GameItems.WoodId, CurrencyItemId = GameItems.CoinsId, Quantity = 5 },
        new() { TargetItemId = GameItems.CoalId, CurrencyItemId = GameItems.CoinsId, Quantity = 25 },
        new() { TargetItemId = GameItems.IronOreId, CurrencyItemId = GameItems.CoinsId, Quantity = 15, },
        new() { TargetItemId = GameItems.SilverOreId, CurrencyItemId = GameItems.CoinsId, Quantity = 35, },
        new() { TargetItemId = GameItems.GoldOreId, CurrencyItemId = GameItems.CoinsId, Quantity = 85, },

        //equipment
        new() { TargetItemId = GameItems.HammerId, CurrencyItemId = GameItems.CoinsId, Quantity = 80 },
        new() { TargetItemId = GameItems.SteelSwordId, CurrencyItemId = GameItems.CoinsId, Quantity = 220 },
        new() { TargetItemId = GameItems.SilverSwordId, CurrencyItemId = GameItems.CoinsId, Quantity = 270 },
        new() { TargetItemId = GameItems.WoodenShieldId, CurrencyItemId = GameItems.CoinsId, Quantity = 50 },
        new() { TargetItemId = GameItems.IronShieldId, CurrencyItemId = GameItems.CoinsId, Quantity = 230 },
        new() { TargetItemId = GameItems.GoldenCrownId, CurrencyItemId = GameItems.CoinsId, Quantity = 800 },
        
        //special        
        new() { TargetItemId = GameItems.RedCrystalId, CurrencyItemId = GameItems.HammerId, Quantity = 1 },
        new() { TargetItemId = GameItems.RedCrystalId, CurrencyItemId = GameItems.WoodenShieldId, Quantity = 1 },
        new() { TargetItemId = GameItems.RedCrystalId, CurrencyItemId = GameItems.WoodenArrowId, Quantity = 250 },

        new() { TargetItemId = GameItems.YellowCrystalId, CurrencyItemId = GameItems.GoldOreId, Quantity = 5 },
        new() { TargetItemId = GameItems.YellowCrystalId, CurrencyItemId = GameItems.GoldenIngotId, Quantity = 2 },
        new() { TargetItemId = GameItems.YellowCrystalId, CurrencyItemId = GameItems.GoldenCrownId, Quantity = 1 },

        new() { TargetItemId = GameItems.BlueCrystalId, CurrencyItemId = GameItems.CoinsId, Quantity = 3000 },
    };
}
