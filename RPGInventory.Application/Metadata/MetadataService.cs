using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Common.Interfaces;
using System.Text;

namespace RPGInventory.Application.Metadata;

public class MetadataService
{
    private readonly IApplicationDbContext _context;

    public MetadataService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MetadataDto> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        var items = await _context.Items
            .Include(i => i.Prices)
                .ThenInclude(p => p.CurrencyItem)
            .Select(i => new MetadataItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Type = i.Type.ToString(),
                Prices = i.Prices.Select(p => new MetadataPriceDto
                {
                    CurrencyItemId = p.CurrencyItemId,
                    CurrencyItemName = p.CurrencyItem != null ? p.CurrencyItem.Name : string.Empty,
                    Quantity = p.Quantity
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        var recipes = await _context.Recipes
            .Include(r => r.ResultItem)
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.IngredientItem)
            .Select(r => new MetadataRecipeDto
            {
                RecipeId = r.Id,
                ResultItemId = r.ResultItemId,
                ResultItemName = r.ResultItem != null ? r.ResultItem.Name : string.Empty,
                ResultQuantity = r.ResultQuantity,
                Ingredients = r.Ingredients.Select(ri => new MetadataIngredientDto
                {
                    IngredientItemId = ri.IngredientItemId,
                    IngredientItemName = ri.IngredientItem != null ? ri.IngredientItem.Name : string.Empty,
                    Quantity = ri.Quantity
                }).ToList()
            })
            .ToListAsync(cancellationToken);        

        return new MetadataDto
        {
            Items = items,
            Recipes = recipes
        };
    }

    public async Task<string> GetMetadataAsTextAsync(CancellationToken cancellationToken = default)
    {
        var metadataDto = await GetMetadataAsync(cancellationToken);

        var sb = new StringBuilder();

        sb.AppendLine("=== Items (ID, Name, Type) ===");
        foreach (var item in metadataDto.Items)
        {
            sb.AppendLine($"● [{item.Id}] {item.Name} ({item.Type})");
            if (item.Prices.Count != 0)
            {
                sb.Append("  Buy/sell cost: ");
                var priceStrings = item.Prices.Select(p => $"{p.Quantity} {p.CurrencyItemName}");
                sb.AppendLine(string.Join(" + ", priceStrings));
            }
            else
            {
                sb.AppendLine("  Cannot be bought or sold");
            }
            sb.AppendLine("----------------------------------------");
        }

        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine("=== Craft Recipes (ID, ResultItemName, ResultItemQuantity) ===");
        foreach (var recipe in metadataDto.Recipes)
        {
            sb.AppendLine($"● [{recipe.RecipeId}] Recipe: {recipe.ResultItemName} x{recipe.ResultQuantity}");
            sb.Append("  Ingredients: ");
            var ingredientStrings = recipe.Ingredients.Select(ri => $"{ri.IngredientItemName} x{ri.Quantity}");
            sb.AppendLine(string.Join(", ", ingredientStrings));
            sb.AppendLine("----------------------------------------");
        }

        return sb.ToString();
    }
}