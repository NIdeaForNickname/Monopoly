namespace Monopoly;

internal interface IGroupable: IBuyable
{
    public InfrastructureGroup Group { get; }
    public int GetPrice();
    public string GetKey();
}