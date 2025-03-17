namespace Monopoly;

public class CardDeck
{
    private readonly Queue<Card> _cards = new Queue<Card>();
    private readonly Board _board;
    private readonly Random _random = new Random();

    public CardDeck(Board board)
    {
        _board = board;
        InitializeDefaultCards();
        Shuffle();
    }

    private void InitializeDefaultCards()
    {
        // Use method groups to bind actions directly
        AddCard("Go to Jail", "Go directly to Jail", CardAction.GoToJail);
        AddCard("Bank Error", "Collect $100", CardAction.GetPaid100);
        AddCard("Doctor's Fee", "Pay $50", CardAction.Pay50);
    }

    internal void AddCard(string name, string description, Func<Card, bool> action)
    {
        // Create a card and bind the action to its own instance method
        var card = new Card(_board, name, description);
        card.BindOnPickupAction(() => action(card));
        _cards.Enqueue(card);
    }

    internal Card DrawCard()
    {
        if (_cards.Count == 0) Shuffle();
        return _cards.Dequeue();
    }

    public void Shuffle()
    {
        var cards = _cards.ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            int j = _random.Next(i, cards.Count);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
        _cards.Clear();
        foreach (var card in cards) _cards.Enqueue(card);
    }
}