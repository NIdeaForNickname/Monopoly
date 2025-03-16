namespace Monopoly;

internal abstract class Tile
{
    public string TileName { get; private set; }
    public List<Player> Players { get; private set; }
    public uint Position { get; private set; }

    public Tile(string name, uint pos)
    {
        TileName = name;
        Position = pos;
        Players = new List<Player>();
    }

    public abstract TurnResult OnStepped(Player player);
        
    public abstract bool Action(Player player);

    public virtual void SetPlayer(Player player)
    {
        Players.Add(player);
    }

    public virtual bool RemovePlayer(Player player)
    {
        Players.Remove(player);
        return true;
    }
}