namespace RPGInventory.Application.Players;

public class PlayerProfileDto
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}