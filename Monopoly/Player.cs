namespace Monopoly;

internal class Player
{
    public string Name { get; private set; }
    public int Money { get; private set; }
    public Tile Location { get; private set; }
    private List<IBuyable> properties;

    public Player(string name, int money = 1500)
    {
        Name = name;
        Money = money;
        properties = new List<IBuyable>();
    }

    public List<IBuyable> PropertiesList()
    {
        return new List<IBuyable>(properties);
    }
        
    public bool MoveTo(Tile location)
    {
        if (Location != null)
        {
            if (!Location.RemovePlayer(this))
            {
                return false;
            }
        }
        Location = location;
        location.SetPlayer(this);
        return true;
    }

    public void AddProperty(IBuyable property)
    {
        properties.Add(property);
    }

    public void RemoveProperty(IBuyable property)
    {
        properties.Remove(property);
    }

    public bool OwnsProperty(IBuyable property)
    {
        return properties.Contains(property);
    }

    public bool AcceptTrade(Transaction tr)
    {
        return tr.Execute();
    }

    public void ReceiveMoney(int amount)
    {
        Money += amount;
    }
}