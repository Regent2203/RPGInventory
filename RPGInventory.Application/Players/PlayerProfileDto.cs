namespace RPGInventory.Application.Players;

public class PlayerProfileDto
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<PlayerInventoryItemDto> Inventory { get; set; } = new();
}

public class PlayerInventoryItemDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public int Quantity { get; set; }
}