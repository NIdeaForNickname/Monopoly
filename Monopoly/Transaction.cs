namespace Monopoly;

class Transaction
{
    public class ItemList
    {
        public int Money { get; set; }
        public List<IBuyable> Properties { get; set; } = new List<IBuyable>();
    }

    public Player? Requester { get; private set; } // Can be null for bank transactions
    public Player? Acceptant { get; private set; } // Can be null for bank transactions
    public ItemList RequesterItems { get; private set; }
    public ItemList AcceptantItems { get; private set; }

    public Transaction(Player? requester, Player? acceptant, ItemList requesterItems, ItemList acceptantItems)
    {
        Requester = requester;
        Acceptant = acceptant;
        RequesterItems = requesterItems;
        AcceptantItems = acceptantItems;
    }

    public bool IsValid()
    {
        if (Requester != null && Requester.Money < RequesterItems.Money) return false;
        if (Acceptant != null && Acceptant.Money < AcceptantItems.Money) return false;

        if (Requester != null)
        {
            foreach (var prop in RequesterItems.Properties)
            {
                if (!Requester.OwnsProperty(prop)) return false;
                if (Acceptant != null && Acceptant.OwnsProperty(prop)) return false;
            }
        }

        if (Acceptant != null)
        {
            foreach (var prop in AcceptantItems.Properties)
            {
                if (!Acceptant.OwnsProperty(prop)) return false;
                if (Requester != null && Requester.OwnsProperty(prop)) return false;
            }
        }

        return true;
    }

    public bool Execute()
    {
        if (!IsValid()) return false;

        Requester?.ReceiveMoney(-RequesterItems.Money);
        Acceptant?.ReceiveMoney(-AcceptantItems.Money);

        Requester?.ReceiveMoney(AcceptantItems.Money);
        Acceptant?.ReceiveMoney(RequesterItems.Money);

        foreach (var prop in RequesterItems.Properties)
        {
            Console.WriteLine(prop.MoveProperty(Requester, Acceptant));
        }

        foreach (var prop in AcceptantItems.Properties)
        {
            Console.WriteLine(prop.MoveProperty(Acceptant, Requester));
        }
            

        return true;
    }
}