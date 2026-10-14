namespace RPGInventory.Application.Players;

public class RegisterPlayerRequest
{
    public string Username { get; set; } = string.Empty;
}

public class RegisterPlayerResponse
{
    public string Message { get; set; } = string.Empty;
    public Guid PlayerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}