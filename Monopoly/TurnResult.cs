namespace Monopoly;

public class TurnResult
{
    public string Message { get; set; }
    public bool CanBuy { get; set; } = false;
    public int MoneyNeeded { get; set; } = 0;
    public string Owner { get; set; }
    public string City { get; set; }
    public bool MandatoryAction { get; set; } = false;
}