namespace RPGInventory.Domain.Entities;

public class Recipe
{
    public int Id { get; set; }

    public int ResultItemId { get; set; }
    public Item? ResultItem { get; set; }

    public int ResultQuantity { get; set; }

    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
}