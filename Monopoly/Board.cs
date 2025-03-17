using System.Globalization;
using Utilities;
namespace Monopoly;

public class Board
{
    private readonly List<CityGroup> _cityGroups = new List<CityGroup>();
    internal readonly HybridList<Tile> _landmarks;
    internal readonly HybridList<Player> _players;
    private Player _currentPlayer;
    public bool ActionDone { get; private set; }
    public bool MovementDone { get; private set; }
    public bool Mandatory { get; private set; }
    public TurnResult TurnRes { get; private set; }

    public Board()
    {
        _landmarks = new HybridList<Tile>();
        _players = new HybridList<Player>();
    }

    // Initialize a test board with players and tiles
    public void SetupTestBoard()
    {
        // Create players
        var player1 = new Player("Alice", 1500);
        var player2 = new Player("Bob", 1500);
        
        // Create all property groups
        var brownGroup = new CityGroup(50, 50);
        var lightBlueGroup = new CityGroup(50, 50);
        var pinkGroup = new CityGroup(100, 100);
        var orangeGroup = new CityGroup(100, 100);
        var redGroup = new CityGroup(150, 150);
        var yellowGroup = new CityGroup(150, 150);
        var greenGroup = new CityGroup(200, 200);
        var darkBlueGroup = new CityGroup(200, 200);
        
        // Create utility/railroad groups
        var utilityGroup = new InfrastructureGroup(new List<IGroupable>(), new List<int> {4, 10});
        var railroadGroup = new InfrastructureGroup(new List<IGroupable>(), new List<int> {25, 50, 100, 200});
        
        // Create card decks
        var communityChestDeck = new CardDeck(this);
        var chanceDeck = new CardDeck(this);
        
        // Create special tiles
        var jail = new Jail("Jail", 10);
        var goToJail = new GoToJail("Go To Jail", 30, jail);

        // Create all 40 tiles in board order
        _landmarks.Add("GO", new SimpleGiver("GO", 0, 200));
        AddCity("Mediterranean Avenue", 1, 60, brownGroup);
        AddCardGiver("Community Chest", 2, communityChestDeck);
        AddCity("Baltic Avenue", 3, 60, brownGroup);
        _landmarks.Add("Income Tax", new SimpleGiver("Income Tax", 4, -200));
        AddRailroad("Reading Railroad", 5, railroadGroup);
        AddCity("Oriental Avenue", 6, 100, lightBlueGroup);
        AddCardGiver("Chance", 7, chanceDeck);
        AddCity("Vermont Avenue", 8, 100, lightBlueGroup);
        AddCity("Connecticut Avenue", 9, 120, lightBlueGroup);
        _landmarks.Add("Jail", jail);
        AddCity("St. Charles Place", 11, 140, pinkGroup);
        AddUtility("Electric Company", 12, utilityGroup);
        AddCity("States Avenue", 13, 140, pinkGroup);
        AddCity("Virginia Avenue", 14, 160, pinkGroup);
        AddRailroad("Pennsylvania Railroad", 15, railroadGroup);
        AddCity("St. James Place", 16, 180, orangeGroup);
        AddCardGiver("Community Chest", 17, communityChestDeck);
        AddCity("Tennessee Avenue", 18, 180, orangeGroup);
        AddCity("New York Avenue", 19, 200, orangeGroup);
        _landmarks.Add("Free Parking", new SimpleGiver("Free Parking", 20, 0));
        AddCity("Kentucky Avenue", 21, 220, redGroup);
        AddCardGiver("Chance", 22, chanceDeck);
        AddCity("Indiana Avenue", 23, 220, redGroup);
        AddCity("Illinois Avenue", 24, 240, redGroup);
        AddRailroad("B. & O. Railroad", 25, railroadGroup);
        AddCity("Atlantic Avenue", 26, 260, yellowGroup);
        AddCity("Ventnor Avenue", 27, 260, yellowGroup);
        AddUtility("Water Works", 28, utilityGroup);
        AddCity("Marvin Gardens", 29, 280, yellowGroup);
        _landmarks.Add("Go To Jail", goToJail);
        AddCity("Pacific Avenue", 31, 300, greenGroup);
        AddCity("North Carolina Avenue", 32, 300, greenGroup);
        AddCardGiver("Community Chest", 33, communityChestDeck);
        AddCity("Pennsylvania Avenue", 34, 320, greenGroup);
        AddRailroad("Short Line Railroad", 35, railroadGroup);
        AddCardGiver("Chance", 36, chanceDeck);
        AddCity("Park Place", 37, 350, darkBlueGroup);
        _landmarks.Add("Luxury Tax", new SimpleGiver("Luxury Tax", 38, -100));
        AddCity("Boardwalk", 39, 400, darkBlueGroup);

        // Initialize players
        _players.Add("Alice", player1);
        _players.Add("Bob", player2);
        foreach (var player in _players.Values)
        {
            player.MoveTo(_landmarks[0]);
        }
        
        _currentPlayer = player1;
        MovementDone = false;
    }

// Helper methods for tile creation
    private void AddCity(string name, uint pos, int price, CityGroup group)
    {
        var city = new City(name, pos, price, group);
        _landmarks.Add(name, city);
        group.Cities.Add(city);
    }

    private void AddRailroad(string name, uint pos, InfrastructureGroup group)
    {
        var railroad = new Infrastructure(name, pos, 200, group);
        _landmarks.Add(name, railroad);
        group.infrastructure.Add(railroad);
    }

    private void AddUtility(string name, uint pos, InfrastructureGroup group)
    {
        var utility = new Infrastructure(name, pos, 150, group);
        _landmarks.Add(name, utility);
        group.infrastructure.Add(utility);
    }

    private void AddCardGiver(string name, uint pos, CardDeck deck)
    {
        _landmarks.Add($"{name} ({pos})", new CardGiver(name, pos, deck));
    }

    public TurnResult MovePlayer(int roll)
    {
        TurnResult result;
        if (!MovementDone){
            MovementDone = true;
            ActionDone = false;
            if (_currentPlayer.MoveTo(
                    _landmarks[Convert.ToInt32((_currentPlayer.Location.Position + roll) % _landmarks.Count)]))
            {
                result = _currentPlayer.Location.OnStepped(_currentPlayer);
                Mandatory = result.MandatoryAction;
                TurnRes = result;
                return TurnRes;
            }
            TurnRes = _currentPlayer.Location.OnStepped(_currentPlayer);
            return TurnRes;
        }
        throw new Exception("Wrong move");
    }

    public bool NextPlayer()
    {
        if (!(!ActionDone && Mandatory) && MovementDone)
        {
            _currentPlayer = _players[(_players.IndexOf(_currentPlayer) + 1) % _players.Count];
            MovementDone = false;
            return true;
        }
        return false;
    }

    public bool DoAction()
    {
        if (!ActionDone && MovementDone)
        {
            if (_currentPlayer.Location.Action(_currentPlayer))
            {
                ActionDone = true;
                return true;
            }
        }
        return false;
    }

    public string GetBoardScheme()
    {
        string temp = "";
        foreach (var i in _landmarks)
        {
            temp += i.TileName + ", ";
        }
        temp += "\n";
        foreach (var i in _players)
        {
            temp += i.Name + " on " + i.Location.TileName + ", ";
        }
        return temp;
    }

    public string CurrentPlayer()
    {
        return _currentPlayer.Name;
    } 
    
    internal Tile GetLandmark(string propertyName)
    {
        return _landmarks[propertyName];
    }

    internal Player GetPlayer()
    {
        return _currentPlayer;
    }
}