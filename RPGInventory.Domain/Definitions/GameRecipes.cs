using RPGInventory.Domain.Entities;

namespace RPGInventory.Domain.Definitions;

public static class GameRecipes
{
    //resources
    public const int IronIngotRecipeId = 2001;
    public const int SilverIngotRecipeId = 2002;
    public const int GoldenIngotRecipeId = 2003;
    public const int SteelIngotRecipeId = 2004;

    //equipment
    public const int WoodenArrowRecipeId = 3001;
    public const int HammerRecipeId = 3002;
    public const int SteelSwordRecipeId = 3003;
    public const int SilverSwordRecipeId = 3004;
    public const int WoodenShieldRecipeId = 3005;
    public const int IronShieldRecipeId = 3006;
    public const int GoldenCrownRecipeId = 3007;

    //special
    public const int YellowCrystalRecipeId = 4001;
    public const int OrbOfPowerRecipeId = 4002;


    public static readonly IReadOnlyList<Recipe> All = new List<Recipe>
    {
        //resources
        new Recipe()
        {
            Id = IronIngotRecipeId,
            ResultItemId = GameItems.IronIngotId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = IronIngotRecipeId, IngredientItemId = GameItems.IronOreId, Quantity = 3 },
            }
        },
        new Recipe()
        {
            Id = SilverIngotRecipeId,
            ResultItemId = GameItems.SilverIngotId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = SilverIngotRecipeId, IngredientItemId = GameItems.SilverOreId, Quantity = 3 },
            }
        },
        new Recipe()
        {
            Id = GoldenIngotRecipeId,
            ResultItemId = GameItems.GoldenIngotId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = GoldenIngotRecipeId, IngredientItemId = GameItems.GoldOreId, Quantity = 3 },
            }
        },
        new Recipe()
        {
            Id = SteelIngotRecipeId,
            ResultItemId = GameItems.SteelIngotId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = SteelIngotRecipeId, IngredientItemId = GameItems.IronOreId, Quantity = 4 },
                new() { RecipeId = SteelIngotRecipeId, IngredientItemId = GameItems.CoalId, Quantity = 1 },
            }
        },

        //equipment
        new Recipe()
        {
            Id = WoodenArrowRecipeId,
            ResultItemId = GameItems.WoodenArrowId,
            ResultQuantity = 50,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = WoodenArrowRecipeId, IngredientItemId = GameItems.WoodId, Quantity = 1 },
            }
        },
        new Recipe()
        {
            Id = HammerRecipeId,
            ResultItemId = GameItems.HammerId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = HammerRecipeId, IngredientItemId = GameItems.WoodId, Quantity = 1 },
                new() { RecipeId = HammerRecipeId, IngredientItemId = GameItems.IronIngotId, Quantity = 1 },
            }
        },
        new Recipe()
        {
            Id = SteelSwordRecipeId,
            ResultItemId = GameItems.SteelSwordId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = SteelSwordRecipeId, IngredientItemId = GameItems.SteelIngotId, Quantity = 2 },
            }
        },
        new Recipe()
        {
            Id = SilverSwordRecipeId,
            ResultItemId = GameItems.SilverSwordId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = SilverSwordRecipeId, IngredientItemId = GameItems.SilverIngotId, Quantity = 2 },
            }
        },
        new Recipe()
        {
            Id = WoodenShieldRecipeId,
            ResultItemId = GameItems.WoodenShieldId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = WoodenShieldRecipeId, IngredientItemId = GameItems.WoodId, Quantity = 5 },
            }
        },
        new Recipe()
        {
            Id = IronShieldRecipeId,
            ResultItemId = GameItems.IronShieldId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = IronShieldRecipeId, IngredientItemId = GameItems.IronIngotId, Quantity = 4 },
            }
        },
        new Recipe()
        {
            Id = GoldenCrownRecipeId,
            ResultItemId = GameItems.GoldenCrownId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = GoldenCrownRecipeId, IngredientItemId = GameItems.GoldenIngotId, Quantity = 2 },
                new() { RecipeId = GoldenCrownRecipeId, IngredientItemId = GameItems.SteelIngotId, Quantity = 1 },
            }
        },

        //special
        new Recipe()
        {
            Id = OrbOfPowerRecipeId,
            ResultItemId = GameItems.OrbOfPowerId,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new() { RecipeId = OrbOfPowerRecipeId, IngredientItemId = GameItems.RedCrystalId, Quantity = 1 },
                new() { RecipeId = OrbOfPowerRecipeId, IngredientItemId = GameItems.YellowCrystalId, Quantity = 1 },
                new() { RecipeId = OrbOfPowerRecipeId, IngredientItemId = GameItems.BlueCrystalId, Quantity = 1 },
            }
        },
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