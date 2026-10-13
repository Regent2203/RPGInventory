using RPGInventory.Domain.Entities;

namespace RPGInventory.Domain.Definitions;

public static class GameItems
{
    public const int GoldId = 1;
    public const int WoodId = 2;
    public const int IronOreId = 3;
    public const int IronIngotId = 4;
    public const int SteelSwordId = 5;

    public static readonly IReadOnlyList<Item> All = new List<Item>
    {
        new() { Id = GoldId, Name = "Золото", Type = ItemType.Currency, BasePrice = 1 },
        new() { Id = WoodId, Name = "Дерево", Type = ItemType.Resource, BasePrice = 5 },
        new() { Id = IronOreId, Name = "Железная руда", Type = ItemType.Resource, BasePrice = 10 },
        new() { Id = IronIngotId, Name = "Железный слиток", Type = ItemType.Resource, BasePrice = 25 },
        new() { Id = SteelSwordId, Name = "Стальной меч", Type = ItemType.Equipment, BasePrice = 100 }
    };

    public static Item Get(int id)
    {
        var item = All.FirstOrDefault(i => i.Id == id);
        if (item == null) 
            throw new KeyNotFoundException($"{nameof(Item)} with ID {id} does not exist.");

        return new Item { Id = item.Id, Name = item.Name, Type = item.Type, BasePrice = item.BasePrice };
    }
}