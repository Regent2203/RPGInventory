using Microsoft.EntityFrameworkCore;
using RPGInventory.Application.Common.Interfaces;
using RPGInventory.Domain.Entities;

namespace RPGInventory.Application.Players;

public class PlayerService
{
    private readonly IApplicationDbContext _context;

    public PlayerService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterPlayerResponse> RegisterAsync(RegisterPlayerRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        var newPlayer = new Player
        {
            Id = Guid.NewGuid(),
            Username = request.Username.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Players.Add(newPlayer);

        var startingResources = new List<InventoryItem>
        {
            new InventoryItem { Id = Guid.NewGuid(), PlayerId = newPlayer.Id, ItemId = 1, Quantity = 100 },
            new InventoryItem { Id = Guid.NewGuid(), PlayerId = newPlayer.Id, ItemId = 2, Quantity = 10 }
        };

        _context.InventoryItems.AddRange(startingResources);

        var historyLogs = startingResources.Select(resource => new InventoryHistory
        {
            PlayerId = newPlayer.Id,
            ItemId = resource.ItemId,
            QuantityChanged = resource.Quantity,
            OperationType = InventoryOperationType.Registration,
            Timestamp = DateTime.UtcNow
        });

        _context.InventoryHistories.AddRange(historyLogs);

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterPlayerResponse
        {
            PlayerId = newPlayer.Id,
            Username = newPlayer.Username,
            CreatedAt = newPlayer.CreatedAt
        };
    }

    public async Task<PlayerProfileDto> GetProfileAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var player = await _context.Players
            .Include(p => p.InventoryItems)
                .ThenInclude(ii => ii.Item)
            .FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);

        if (player == null)
        {
            throw new KeyNotFoundException($"Player not found.");
        }

        return new PlayerProfileDto
        {
            PlayerId = player.Id,
            Username = player.Username,
            CreatedAt = player.CreatedAt,
            Inventory = player.InventoryItems.Select(ii => new PlayerInventoryItemDto
            {
                ItemId = ii.ItemId,
                ItemName = ii.Item?.Name ?? string.Empty,
                ItemType = ii.Item?.Type.ToString() ?? string.Empty,
                Quantity = ii.Quantity
            }).ToList()
        };
    }

    public async Task<List<InventoryHistoryDto>> GetHistoryAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var playerExists = await _context.Players.AnyAsync(p => p.Id == playerId, cancellationToken);
        if (!playerExists)
        {
            throw new KeyNotFoundException($"Player not found.");
        }

        var history = await _context.InventoryHistories
            .Where(h => h.PlayerId == playerId)
            .Include(h => h.Item)
            .OrderByDescending(h => h.Timestamp)
            .Select(h => new InventoryHistoryDto
            {
                Id = h.Id,
                ItemId = h.ItemId,
                ItemName = h.Item != null ? h.Item.Name : string.Empty,
                ItemType = h.Item != null ? h.Item.Type.ToString() : string.Empty,
                QuantityChanged = h.QuantityChanged,                
                Operation = h.OperationType.ToString(),
                Timestamp = h.Timestamp
            }).ToListAsync(cancellationToken);

        return history;
    }
}