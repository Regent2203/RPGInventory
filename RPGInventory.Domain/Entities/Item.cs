namespace RPGInventory.Domain.Entities;

public enum ItemType
{
    Unassigned = 0,
    Currency = 1,
    Resource = 2,
    Equipment = 3,
    Special = 4,
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ItemType Type { get; set; }

    public ICollection<ItemPrice> Prices { get; set; } = new List<ItemPrice>();
}
