namespace RPGInventory.Domain.Entities;

public enum InventoryOperationType
{
    Registration = 1, // Получено при регистрации
    Purchase = 2,     // Куплено в магазине
    Sale = 3,         // Продано в магазин
    CraftConsume = 4, // Потрачено на крафт (ингредиент)
    CraftProduce = 5  // Получено в результате крафта
}

public class InventoryHistory
{
    public long Id { get; set; } // Для логов лучше использовать bigint (long)
    public Guid PlayerId { get; set; }
    public Player? Player { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int QuantityChanged { get; set; } // Например, +1 (меч) или -5 (дерево)
    public InventoryOperationType OperationType { get; set; }
    public DateTime Timestamp { get; set; }
}