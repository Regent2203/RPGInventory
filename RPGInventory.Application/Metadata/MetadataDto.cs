namespace RPGInventory.Application.Metadata;

public class MetadataDto
{
    public List<MetadataItemDto> Items { get; set; } = new();
    public List<MetadataRecipeDto> Recipes { get; set; } = new();
}

public class MetadataItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<MetadataPriceDto> Prices { get; set; } = new();
}

public class MetadataPriceDto
{
    public int CurrencyItemId { get; set; }
    public string CurrencyItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class MetadataRecipeDto
{
    public int RecipeId { get; set; }
    public int ResultItemId { get; set; }
    public string ResultItemName { get; set; } = string.Empty;
    public int ResultQuantity { get; set; }
    public List<MetadataIngredientDto> Ingredients { get; set; } = new();
}

public class MetadataIngredientDto
{
    public int IngredientItemId { get; set; }
    public string IngredientItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
