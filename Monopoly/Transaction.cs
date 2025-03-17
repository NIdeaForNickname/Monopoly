namespace Monopoly;

class Transaction
{
    public class ItemList
    {
        public int Money { get; set; }
        public List<IBuyable> Properties { get; set; } = new List<IBuyable>();
    }

    public Player? From { get; private set; } // Can be null for bank transactions
    public Player? Towards { get; private set; } // Can be null for bank transactions
    public ItemList FromItems { get; private set; }
    public ItemList TowardsItems { get; private set; }

    public Transaction(Player? from, Player? towards, ItemList fromItems, ItemList towardsItems)
    {
        From = from;
        Towards = towards;
        FromItems = fromItems;
        TowardsItems = towardsItems;
    }

    public bool IsValid()
    {
        if (From != null && From.Money < FromItems.Money) return false;
        if (Towards != null && Towards.Money < TowardsItems.Money) return false;

        if (From != null)
        {
            foreach (var prop in FromItems.Properties)
            {
                if (!From.OwnsProperty(prop)) return false;
                if (Towards != null && Towards.OwnsProperty(prop)) return false;
            }
        }

        if (Towards != null)
        {
            foreach (var prop in TowardsItems.Properties)
            {
                if (!Towards.OwnsProperty(prop)) return false;
                if (From != null && From.OwnsProperty(prop)) return false;
            }
        }

        return true;
    }

    public bool Execute()
    {
        if (!IsValid()) return false;

        From?.ReceiveMoney(-FromItems.Money);
        Towards?.ReceiveMoney(-TowardsItems.Money);

        From?.ReceiveMoney(TowardsItems.Money);
        Towards?.ReceiveMoney(FromItems.Money);

        foreach (var prop in FromItems.Properties)
        {
            Console.WriteLine(prop.MoveProperty(From, Towards));
        }

        foreach (var prop in TowardsItems.Properties)
        {
            Console.WriteLine(prop.MoveProperty(Towards, From));
        }
            

        return true;
    }
}