using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Common.Interfaces;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<InventoryHistory> InventoryHistories => Set<InventoryHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.PlayerId, e.ItemId }).IsUnique();

            entity.HasOne(d => d.Player)
                .WithMany(p => p.InventoryItems)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Item)
                .WithMany()
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.RecipeId, e.IngredientItemId }).IsUnique();

            entity.HasOne(d => d.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.IngredientItem)
                .WithMany()
                .HasForeignKey(d => d.IngredientItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasOne(d => d.ResultItem)
                .WithMany()
                .HasForeignKey(d => d.ResultItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InventoryHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(d => d.Player).WithMany().HasForeignKey(d => d.PlayerId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Item).WithMany().HasForeignKey(d => d.ItemId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}