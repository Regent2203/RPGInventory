namespace RPGInventory.Domain.Entities;

public class InventoryItem
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public Player? Player { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int Quantity { get; set; }
}