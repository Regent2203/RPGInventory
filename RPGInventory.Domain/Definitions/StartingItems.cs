namespace RPGInventory.Domain.Definitions;

public record StartingItem(int ItemId, int Quantity);

public static class StartingItems
{
    public static readonly IReadOnlyList<StartingItem> All = new List<StartingItem>
    {
        new StartingItem(GameItems.CoinsId, 2500 ),
    };
}