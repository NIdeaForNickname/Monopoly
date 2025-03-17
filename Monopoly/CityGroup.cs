namespace Monopoly;

internal class CityGroup
{
    public List<City> Cities { get; } = new List<City>();
    public int HouseCost { get; }
    public int HotelCost { get; }

    public CityGroup(int houseCost, int hotelCost)
    {
        HouseCost = houseCost;
        HotelCost = hotelCost;
    }
}