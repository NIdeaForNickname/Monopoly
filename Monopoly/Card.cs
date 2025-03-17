using System.Diagnostics.CodeAnalysis;

namespace Monopoly;

public enum CardTypes { BonusField, ChestField }

internal partial class Card
{
    private Board _board;
    public Player? Owner { get; set; }
    public string CardName { get; private set; }
    public string CardDescription { get; private set; }
    private OnPickupDel _onPickupVar;
    private ExecuteDel _executeVar;
    

    // ReSharper disable once IdentifierTypo
    public Card([NotNull]Board bd, string cardName, string cardDescription, OnPickupDel? pdel = null, ExecuteDel? edel = null)
    {
        this.CardName = cardName;
        this.CardDescription = cardDescription;
        Owner = null;
        _board = bd;
    }
    
    public bool OnPickup() => _onPickupVar();
    public bool Execute() => _executeVar();
    
    public void BindOnPickupAction(OnPickupDel action)
    {
        // Allow binding the action post-construction
        _onPickupVar = action;
    }
    
    public delegate bool OnPickupDel();
    public delegate bool ExecuteDel();
}