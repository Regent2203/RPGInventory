using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Crafting;
using RPGInventory.Domain.Definitions;
using RPGInventory.Domain.Entities;
using RPGInventory.Infrastructure.Persistence;
using Xunit;

namespace RPGInventory.Application.UnitTests.Crafting;

public class CraftServiceTests
{
    private ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CraftAsync_ShouldCreateItem_WhenPlayerHasEnoughIngredients()
    {
        using var context = CreateInMemoryDbContext();

        context.Items.AddRange(GameItems.All);
        context.Recipes.AddRange(GameRecipes.All);

        // Player with enough items to craft steel sword
        var playerId = Guid.NewGuid();
        var player = new Player
        {
            Id = playerId,
            Username = "Norm player",
            InventoryItems = new List<InventoryItem>
            {
                new() { Id = Guid.NewGuid(), PlayerId = playerId, ItemId = GameItems.IronIngotId, Quantity = 2 },
                new() { Id = Guid.NewGuid(), PlayerId = playerId, ItemId = GameItems.WoodId, Quantity = 10 }
            }
        };
        context.Players.Add(player);
        await context.SaveChangesAsync();

        var sut = new CraftService(context);
        var request = new CraftItemRequest { PlayerId = playerId, RecipeId = GameRecipes.SteelSwordRecipeId };
        
        var result = await sut.CraftAsync(request);

        result.Should().NotBeNull();
        result.CraftedItemName.Should().Be(GameItems.Get(GameItems.SteelSwordId).Name);

        var woodInDb = await context.InventoryItems.FirstOrDefaultAsync(ii => ii.PlayerId == playerId && ii.ItemId == GameItems.WoodId);
        woodInDb!.Quantity.Should().Be(5);

        var ingotsInDb = await context.InventoryItems.FirstOrDefaultAsync(ii => ii.PlayerId == playerId && ii.ItemId == GameItems.IronIngotId);
        ingotsInDb.Should().BeNull();
    }

    [Fact]
    public async Task CraftAsync_ShouldThrowInvalidOperationException_WhenIngredientsAreMissing()
    {
        using var context = CreateInMemoryDbContext();

        context.Items.AddRange(GameItems.All);
        context.Recipes.AddRange(GameRecipes.All);

        // Player with not enough items to craft steel sword
        var playerId = Guid.NewGuid();
        var player = new Player
        {
            Id = playerId,
            Username = "Poor player",
            InventoryItems = new List<InventoryItem>
            {
                new() { Id = Guid.NewGuid(), PlayerId = playerId, ItemId = GameItems.WoodId, Quantity = 10 }
            }
        };
        context.Players.Add(player);
        await context.SaveChangesAsync();

        var sut = new CraftService(context);
        var request = new CraftItemRequest { PlayerId = playerId, RecipeId = GameRecipes.SteelSwordRecipeId };

        Func<Task> act = async () => await sut.CraftAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Not enough items for craft: Железный слиток. Need: 2.");
    }
}
