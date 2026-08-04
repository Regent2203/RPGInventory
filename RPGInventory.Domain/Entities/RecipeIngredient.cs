namespace RPGInventory.Domain.Entities;

public class RecipeIngredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

    public int IngredientItemId { get; set; }
    public Item? IngredientItem { get; set; }

    public int Quantity { get; set; }
}