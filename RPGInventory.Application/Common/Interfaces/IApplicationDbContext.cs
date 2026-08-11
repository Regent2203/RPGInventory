using Microsoft.EntityFrameworkCore;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Player> Players { get; }
    DbSet<Item> Items { get; }
    DbSet<InventoryItem> InventoryItems { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<RecipeIngredient> RecipeIngredients { get; }
    DbSet<InventoryHistory> InventoryHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}