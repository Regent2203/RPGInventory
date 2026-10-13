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
        new() { Id = GoldId, Name = "Золото", Type = ItemType.Currency, },
        new() { Id = WoodId, Name = "Дерево", Type = ItemType.Resource, },
        new() { Id = IronOreId, Name = "Железная руда", Type = ItemType.Resource },
        new() { Id = IronIngotId, Name = "Железный слиток", Type = ItemType.Resource },
        new() { Id = SteelSwordId, Name = "Стальной меч", Type = ItemType.Equipment }
    };

    public static Item Get(int id)
    {
        var item = All.FirstOrDefault(i => i.Id == id);
        if (item == null) 
            throw new KeyNotFoundException($"{nameof(Item)} with ID {id} does not exist.");

        return new Item { Id = item.Id, Name = item.Name, Type = item.Type, };
    }
}