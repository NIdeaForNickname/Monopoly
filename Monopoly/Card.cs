using System.Diagnostics.CodeAnalysis;

namespace Monopoly;

public enum CardTypes { BonusField, ChestField }

internal partial class Card
{
    private Board _board;
    public Player? Owner { get; private set; }
    public CardTypes CardType { get; private set; }
    public string CardName { get; private set; }
    public string CardDescription { get; private set; }
    private readonly OnPickupDel _onPickupVar;
    private readonly ExecuteDel _executeVar;
    

    // ReSharper disable once IdentifierTypo
    public Card([NotNull]Board bd, CardTypes cardType, string cardName, string cardDescription, OnPickupDel? pdel = null, ExecuteDel? edel = null)
    {
        this.CardType = cardType;
        this.CardName = cardName;
        this.CardDescription = cardDescription;
        _onPickupVar = pdel ?? (() => false);
        _executeVar = edel ?? (() => false);
        Owner = null;
        _board = bd;
    }

    public bool OnPickup() => _onPickupVar();
    public bool Execute() => _executeVar();
    
    public delegate bool OnPickupDel();
    public delegate bool ExecuteDel();
}