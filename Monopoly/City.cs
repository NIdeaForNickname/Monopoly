namespace Monopoly;

internal class City : Tile, IBuyable
{
    public CityGroup Group { get; }
    
    public int Price { get; }
    public Player? Owner { get; private set; }
    public int BasePrice { get; }
    public int Houses { get; private set; }
    public bool HasHotel { get; private set; }
    
    // Rent multipliers based on development state
    private static readonly int[] _rentMultipliers = { 1, 5, 15, 40, 80, 125 };

    public City(string name, uint pos, int basePrice, CityGroup group) : base(name, pos)
    {
        BasePrice = basePrice;
        Group = group;
        group.Cities.Add(this);
    }

    public override TurnResult OnStepped(Player player)
    {
        if (Owner == null)
        {
            return new TurnResult
            {
                Message = $"{TileName} is available for purchase (${BasePrice})",
                CanBuy = true,
                City = TileName,
                MandatoryAction = false
            };
        } else if (player == Owner)
        {
            return new TurnResult
            {
                Message = $"{player.Name} landed on their property and {(DevelopmentPossible()? "can" : "cannot")} develop their proprety",
                CanBuy = true,
                City = TileName,
                MandatoryAction = false
            };
        }

        var rent = CalculateRent();
        return new TurnResult
        {
            Message = $"{player.Name} must pay ${rent} to {Owner.Name} for {TileName}",
            MoneyNeeded = rent,
            Owner = Owner.Name,
            MandatoryAction = true
        };
    }

    public override bool Action(Player player)
    {
        if (Owner == null)
        {
            var transaction = new Transaction(
                null,
                player,
                new Transaction.ItemList { Properties = new List<IBuyable> { this } },
                new Transaction.ItemList { Money = BasePrice }
            );
            return transaction.Execute();
        }
        else if (Owner != player)
        {
            var rent = CalculateRent();
            var transaction = new Transaction(
                player,
                Owner,
                new Transaction.ItemList { Money = rent },
                new Transaction.ItemList { }
            );
            return transaction.Execute();
        }
        
        return TryDevelopProperty();
    }

    private int CalculateRent()
    {
        if (HasHotel) return BasePrice * _rentMultipliers[5];
        return BasePrice * _rentMultipliers[Houses];
    }

    private bool DevelopmentPossible()
    {
        return Group.Cities.All(c => c.Owner == Owner) && !HasHotel;
    }
    
    private bool TryDevelopProperty()
    {
        if (!DevelopmentPossible()) return false;

        var developmentCost = HasHotel ? Group.HotelCost : Group.HouseCost;

        var transaction = new Transaction(
            Owner,
            null,
            new Transaction.ItemList { Money = developmentCost },
            new Transaction.ItemList { }
        );

        if (!transaction.Execute()) return false;

        if (Houses < 4)
        {
            Houses++;
        }
        else
        {
            Houses = 0;
            HasHotel = true;
        }

        return true;
    }

    public bool MoveProperty(Player from, Player to)
    {
        if (Owner != from) return false;
        
        Owner?.RemoveProperty(this);
        to?.AddProperty(this);
        Owner = to;
        return true;
    }
}