namespace RPGInventory.Application.Crafting;

public class CraftItemRequest
{
    public Guid PlayerId { get; set; }
    public int RecipeId { get; set; }
}

public class CraftItemResponse
{
    public string Message { get; set; } = string.Empty;
    public string CraftedItemName { get; set; } = string.Empty;
    public int CraftedQuantity { get; set; }
    public List<InventoryItemDto> UpdatedInventory { get; set; } = new();
}

public class InventoryItemDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}