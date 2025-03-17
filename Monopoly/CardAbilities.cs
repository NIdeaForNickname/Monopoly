namespace Monopoly;

internal partial class Card
{
    public bool Action_GoToJail() { return Owner.MoveTo(_board.GetLandmark("Jail")); }
    public bool Action_GetPaid100()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { Money = 100 },
            new Transaction.ItemList { }
        );
        return tr.Execute();
    }
    public bool Action_GetPaid20()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { Money = 100 },
            new Transaction.ItemList { }
        );
        return tr.Execute();
    }
    public bool Action_GetPaid10()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { Money = 100 },
            new Transaction.ItemList { }
        );
        return tr.Execute();
    }
    public bool Action_CollectFromPlayers()
    {
        Transaction tr;
        foreach (var i in _board._players.Values)
        {
            tr = new Transaction(
                i,
                Owner,
                new Transaction.ItemList { Money = 50 },
                new Transaction.ItemList { }
            );
            if (i != Owner)
            {
                tr.Execute();
            }
        }
        return true;
    }
    public bool Action_GetPaid200()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { Money = 200 },
            new Transaction.ItemList { }
        );
        return tr.Execute();
    }
    public bool Action_GetPaid25()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { Money = 100 },
            new Transaction.ItemList { }
        );
        return tr.Execute();
    }
    public bool Action_Pay10()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { },
            new Transaction.ItemList { Money = 100 }
        );
        return tr.Execute();
    }
    public bool Action_Pay50()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { },
            new Transaction.ItemList { Money = 50 }
        );
        return tr.Execute();
    }
    public bool Action_Pay150()
    {
        Transaction tr = new Transaction(
            null,
            Owner,
            new Transaction.ItemList { },
            new Transaction.ItemList { Money = 150 }
        );
        return tr.Execute();
    }
}

internal static class CardAction
{
    public static bool GoToJail(Card card) => card.Action_GoToJail();
    public static bool GetPaid100(Card card) => card.Action_GetPaid100();
    public static bool Pay50(Card card) => card.Action_Pay50();
    public static bool GetPaid200(Card card) => card.Action_GetPaid200();
    public static bool GetPaid25(Card card) => card.Action_GetPaid25();
    public static bool GetPaid10(Card card) => card.Action_GetPaid10();
    public static bool GetPaid20(Card card) => card.Action_GetPaid20();
    public static bool Pay10(Card card) => card.Action_Pay10();
    public static bool Pay150(Card card) => card.Action_Pay150();
    public static bool CollectFromPlayers(Card card) => card.Action_CollectFromPlayers();
}