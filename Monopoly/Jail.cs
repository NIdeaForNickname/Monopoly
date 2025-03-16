namespace Monopoly;

class Jail: Tile
{
    public Dictionary<string, JailState> JailPlayers { get; private set; }
        
    public class JailState
    {
        public Player player { get; set; }
        public int timeLeft { get; set; }
    }

    public override bool Action(Player player)
    {
        if (JailPlayers.TryGetValue(player.Name, out JailState jailState))
        {
            jailState.timeLeft--;
            if (jailState.timeLeft <= 0)
            {
                JailPlayers.Remove(player.Name);
            }
        }
        return true;
    }

    public override bool RemovePlayer(Player player)
    {
        if (JailPlayers.TryGetValue(player.Name, out JailState jailState))
        {
            return false;
        }
        return base.RemovePlayer(player);
    }

    public Jail(string name, uint pos) : base(name, pos)
    {
        JailPlayers = new Dictionary<string, JailState>();
    }

    public override TurnResult OnStepped(Player player)
    {
        if (JailPlayers.TryGetValue(player.Name, out JailState jailState))
        {
            return new TurnResult
            {
                Message = "You Are stuck in prison",
                MandatoryAction = true
            };
        }
        else
        {
            return new TurnResult
            {
                Message = "Just Passing by a prison",
                MandatoryAction = true
            };
        }
    }

    public bool SendToJail(Player player)
    {
        if (player.MoveTo(this))
        {
            JailPlayers.Add(player.Name, new JailState
            {
                player = player,
                timeLeft = 3
            });
            return true;
        }
        return false;
    }
}