namespace RPGInventory.Application.Shop;

public class TradeRequest
{
    public Guid PlayerId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

public class TradeResponse
{
    public string Message { get; set; } = string.Empty;
    public int GoldBalance { get; set; }
    public int ItemInventoryQuantity { get; set; }
}