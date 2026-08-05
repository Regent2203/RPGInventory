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
            throw new ArgumentException("Имя игрока не может быть пустым.");
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

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterPlayerResponse
        {
            PlayerId = newPlayer.Id,
            Username = newPlayer.Username,
            CreatedAt = newPlayer.CreatedAt
        };
    }
}
