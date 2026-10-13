using RPGInventory.Domain.Constants;
using RPGInventory.Domain.Definitions;

namespace RPGInventory.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedInitialDataAsync(ApplicationDbContext context)
    {
        await context.Items.AddRangeAsync(GameItems.All);
        await context.SaveChangesAsync();

        await context.ItemPrices.AddRangeAsync(GamePrices.All);
        await context.SaveChangesAsync();

        await context.Recipes.AddRangeAsync(GameRecipes.All);
        await context.SaveChangesAsync();        
    }
}