namespace Monopoly;

internal interface IBuyable
{
    Player? Owner { get; }
    int Price { get; }
    bool MoveProperty(Player Own, Player Acc);
}