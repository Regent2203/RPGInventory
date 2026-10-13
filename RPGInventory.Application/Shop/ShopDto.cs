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
    public List<UpdatedItemDto> UpdatedItems { get; set; } = new();
}

public class UpdatedItemDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int NewQuantity { get; set; }
}