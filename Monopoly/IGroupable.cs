namespace Monopoly;

internal interface IGroupable: IBuyable
{
    public InfrastructureGroup Group { get; set; }
    public int GetPrice();
    public string GetKey();
}