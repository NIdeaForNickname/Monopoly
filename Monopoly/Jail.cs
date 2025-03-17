namespace Monopoly;

class Jail : Tile
{
    public Dictionary<string, JailState> JailPlayers { get; } = new();
    
    public class JailState
    {
        public Player Player { get; set; }
        public int TimeLeft { get; set; }
    }

    public Jail(string name, uint pos) : base(name, pos) { }

    public override TurnResult OnStepped(Player player)
    {
        if (JailPlayers.ContainsKey(player.Name))
            return new TurnResult { Message = "You're stuck in jail!", MandatoryAction = true };
        
        return new TurnResult { Message = "Just visiting", MandatoryAction = false };
    }

    public bool SendToJail(Player player)
    {
        if (!player.MoveTo(this)) return false;
        
        JailPlayers[player.Name] = new JailState { Player = player, TimeLeft = 3 };
        return true;
    }

    public void ProcessJailTurn(Player player)
    {
        if (JailPlayers.TryGetValue(player.Name, out JailState state))
        {
            state.TimeLeft--;
            if (state.TimeLeft <= 0)
            {
                JailPlayers.Remove(player.Name);
            }
        }
    }

    public override bool Action(Player player)
    {
        ProcessJailTurn(player);
        return true;
    }

    public override bool RemovePlayer(Player player)
    {
        if (JailPlayers.ContainsKey(player.Name))
        {
            return false;
        }
        base.RemovePlayer(player);
        return true;
    }
}