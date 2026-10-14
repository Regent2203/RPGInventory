namespace RPGInventory.Domain.Entities;

public enum InventoryOperationType
{
    Registration = 1,
    Purchase = 2,
    Sale = 3,
    CraftConsume = 4,
    CraftProduce = 5,
}

public class InventoryHistory
{
    public long Id { get; set; }
    public Guid PlayerId { get; set; }
    public Player? Player { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int QuantityChanged { get; set; }
    public InventoryOperationType OperationType { get; set; }
    public DateTime Timestamp { get; set; }
}