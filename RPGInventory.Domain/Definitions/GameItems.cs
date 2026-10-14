using RPGInventory.Domain.Entities;

namespace RPGInventory.Domain.Definitions;

public static class GameItems
{
    //currency
    public const int CoinsId = 1001;

    //resource
    public const int WoodId = 2001;
    public const int CoalId = 2002;
    public const int IronOreId = 2003;
    public const int SilverOreId = 2004;
    public const int GoldOreId = 2005;
    public const int IronIngotId = 2006;
    public const int SilverIngotId = 2007;
    public const int GoldenIngotId = 2008;
    public const int SteelIngotId = 2009;

    //equipment
    public const int WoodenArrowId = 3001;
    public const int HammerId = 3002;
    public const int SteelSwordId = 3003;
    public const int SilverSwordId = 3004;
    public const int WoodenShieldId = 3005;
    public const int IronShieldId = 3006;
    public const int GoldenCrownId = 3007;

    //special
    public const int RedCrystalId = 4001;
    public const int YellowCrystalId = 4002;
    public const int BlueCrystalId = 4003;
    public const int OrbOfPowerId = 4004;


    public static readonly IReadOnlyList<Item> All = new List<Item>
    {
        //currency
        new() { Id = CoinsId, Name = "Coins", Type = ItemType.Currency, },

        //resource
        new() { Id = WoodId, Name = "Wood", Type = ItemType.Resource, },
        new() { Id = CoalId, Name = "Coal", Type = ItemType.Resource, },
        new() { Id = IronOreId, Name = "Iron Ore", Type = ItemType.Resource, },
        new() { Id = SilverOreId, Name = "Silver Ore", Type = ItemType.Resource, },
        new() { Id = GoldOreId, Name = "Gold Ore", Type = ItemType.Resource, },
        new() { Id = IronIngotId, Name = "Iron Ingot", Type = ItemType.Resource, },
        new() { Id = SilverIngotId, Name = "Silver Ingot", Type = ItemType.Resource, },
        new() { Id = GoldenIngotId, Name = "Golden Ingot", Type = ItemType.Resource, },
        new() { Id = SteelIngotId, Name = "Steel Ingot", Type = ItemType.Resource, },

        //equipment
        new() { Id = WoodenArrowId, Name = "Wooden Arrow", Type = ItemType.Equipment },
        new() { Id = HammerId, Name = "Hammer", Type = ItemType.Equipment },
        new() { Id = SteelSwordId, Name = "Steel Sword", Type = ItemType.Equipment },
        new() { Id = SilverSwordId, Name = "Silver Sword", Type = ItemType.Equipment },
        new() { Id = WoodenShieldId, Name = "Wooden Shield", Type = ItemType.Equipment },
        new() { Id = IronShieldId, Name = "Iron Shield", Type = ItemType.Equipment },
        new() { Id = GoldenCrownId, Name = "Golden Crown", Type = ItemType.Equipment },

        //special
        new() { Id = RedCrystalId, Name = "Red Crystal", Type = ItemType.Special },
        new() { Id = YellowCrystalId, Name = "Yellow Crystal", Type = ItemType.Special },
        new() { Id = BlueCrystalId, Name = "Blue Crystal", Type = ItemType.Special },
        new() { Id = OrbOfPowerId, Name = "Orb of Power", Type = ItemType.Special },
    };

    public static Item Get(int id)
    {
        var item = All.FirstOrDefault(i => i.Id == id);
        if (item == null) 
            throw new KeyNotFoundException($"{nameof(Item)} with ID {id} does not exist.");

        return new Item { Id = item.Id, Name = item.Name, Type = item.Type, };
    }
}