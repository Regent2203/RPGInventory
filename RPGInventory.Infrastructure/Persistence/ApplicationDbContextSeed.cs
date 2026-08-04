using Microsoft.EntityFrameworkCore;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedInitialDataAsync(ApplicationDbContext context)
    {
        if (await context.Items.AnyAsync())
            return;

        var gold = new Item { Id = 1, Name = "Золото", Type = ItemType.Currency, BasePrice = 1 };
        var wood = new Item { Id = 2, Name = "Дерево", Type = ItemType.Resource, BasePrice = 5 };
        var ironOre = new Item { Id = 3, Name = "Железная руда", Type = ItemType.Resource, BasePrice = 10 };
        var ironIngot = new Item { Id = 4, Name = "Железный слиток", Type = ItemType.Resource, BasePrice = 25 };
        var steelSword = new Item { Id = 5, Name = "Стальной меч", Type = ItemType.Equipment, BasePrice = 100 };

        await context.Items.AddRangeAsync(gold, wood, ironOre, ironIngot, steelSword);
        await context.SaveChangesAsync();

        var ingotRecipe = new Recipe
        {
            Id = 1,
            ResultItemId = ironIngot.Id,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { IngredientItemId = ironOre.Id, Quantity = 3 }
            }
        };

        var swordRecipe = new Recipe
        {
            Id = 2,
            ResultItemId = steelSword.Id,
            ResultQuantity = 1,
            Ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { IngredientItemId = ironIngot.Id, Quantity = 2 },
                new RecipeIngredient { IngredientItemId = wood.Id, Quantity = 5 }
            }
        };

        await context.Recipes.AddRangeAsync(ingotRecipe, swordRecipe);
        await context.SaveChangesAsync();
    }
}