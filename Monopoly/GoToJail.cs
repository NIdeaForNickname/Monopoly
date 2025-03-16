namespace Monopoly;

internal class GoToJail: Tile
{
    Jail linkedJail;
    public GoToJail(string name, uint pos, Jail jail) : base(name, pos)
    {
        linkedJail = jail;
    }

    public override TurnResult OnStepped(Player player)
    {
        linkedJail.SendToJail(player);
        return linkedJail.OnStepped(player);
    }

    public override bool Action(Player player)
    {
        return true;
    }
}