using Utilities;
namespace Monopoly;

public class Board
{
    internal readonly HybridList<Tile> _landmarks;
    internal readonly HybridList<Player> _players;

    public Board()
    {
        _landmarks = new HybridList<Tile>();
        _players = new HybridList<Player>();
    }

    internal Tile GetLandmark(string name) { return _landmarks[name]; }
    internal Player GetPlayer(string name) { return _players[name]; }
    
    
}