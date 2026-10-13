using RPGInventory.Domain.Entities;

namespace RPGInventory.Domain.Definitions;

public static class GameRecipes
{
    public const int IronIngotRecipeId = 1;
    public const int SteelSwordRecipeId = 2;

    public static readonly IReadOnlyList<Recipe> All = new List<Recipe>
    {
        //1 Железный слиток = 3 Железные руды
        new()
        {
            Id = IronIngotRecipeId,
            ResultItemId = GameItems.IronIngotId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = IronIngotRecipeId, IngredientItemId = GameItems.IronOreId, Quantity = 3 }
            }
        },

        //1 Стальной меч = 2 Железных слитка + 5 Дерева
        new()
        {
            Id = SteelSwordRecipeId,
            ResultItemId = GameItems.SteelSwordId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = SteelSwordRecipeId, IngredientItemId = GameItems.IronIngotId, Quantity = 2 },
                new() { RecipeId = SteelSwordRecipeId, IngredientItemId = GameItems.WoodId, Quantity = 5 }
            }
        }
    };

    public static Recipe Get(int id)
    {
        var recipe = All.FirstOrDefault(r => r.Id == id);
        if (recipe == null) 
            throw new KeyNotFoundException($"{nameof(Recipe)} with ID {id} does not exist.");

        return new Recipe
        {
            Id = recipe.Id,
            ResultItemId = recipe.ResultItemId,
            ResultQuantity = recipe.ResultQuantity,
            Ingredients = recipe.Ingredients.Select(i => new RecipeIngredient
            {
                Id = i.Id,
                RecipeId = i.RecipeId,
                IngredientItemId = i.IngredientItemId,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}