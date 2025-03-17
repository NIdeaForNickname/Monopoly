namespace Monopoly;

internal class CardGiver : Tile
{
    private readonly CardDeck _deck;
    private Card? _activeCard;

    public CardGiver(string name, uint pos, CardDeck deck) : base(name, pos)
    {
        _deck = deck;
    }

    public override TurnResult OnStepped(Player player)
    {
        _activeCard = _deck.DrawCard();
        _activeCard.Owner = player; // Set card owner to current player
        return new TurnResult
        {
            Message = $"Drew card: {_activeCard.CardDescription}",
            MandatoryAction = true
        };
    }

    public override bool Action(Player player)
    {
        if (_activeCard == null) return false;
        
        bool success = _activeCard.Execute();
        _activeCard = null;
        return success;
    }
}
