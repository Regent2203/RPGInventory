namespace RPGInventory.Domain.Entities;

public class ItemPrice
{
    public int Id { get; set; }

    public int TargetItemId { get; set; }
    public Item? TargetItem { get; set; }

    public int CurrencyItemId { get; set; }
    public Item? CurrencyItem { get; set; }

    public int Quantity { get; set; }
}
