namespace Monopoly;

internal class SimpleGiver: Tile
{
    private int Money;

    public SimpleGiver(string name, uint pos, int money) : base(name, pos)
    {
        this.Money = money;
    }

    public override TurnResult OnStepped(Player player)
    {
        if (Money < 0) 
            return new TurnResult
            {
                Message = "You need to pay the price",
                MoneyNeeded = Money,
                MandatoryAction = true
            };
        else
            return new TurnResult
            {
                Message = "You will get some money",
                MoneyNeeded = Money,
                MandatoryAction = true
            };
    }

    public override bool Action(Player player)
    {
        if (Money < 0)
        {
            Transaction tr = new Transaction(
                null,
                player,
                new Transaction.ItemList { },
                new Transaction.ItemList { Money = Money }
            );
            return tr.Execute();
        }
        else
        {
            Transaction tr = new Transaction(
                null,
                player,
                new Transaction.ItemList { Money = Money },
                new Transaction.ItemList { }
            );
            return tr.Execute();
        }
    }
}