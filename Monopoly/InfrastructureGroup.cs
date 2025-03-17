namespace Monopoly;

internal class InfrastructureGroup
{
    internal List<IGroupable> infrastructure;
    private List<int> pricelist;
    
    public InfrastructureGroup(List<IGroupable> infrastructure, List<int> pricelist)
    {
        this.infrastructure = infrastructure;
        this.pricelist = pricelist;
    }
    
    public void AddInfrastructure(IGroupable infrastructure, int price){}

    public int GetPrice(Infrastructure id)
    {
        int temp = -1;
        foreach (var i in infrastructure)
        {
            if (i.GetKey() == id.GetKey() && temp <= pricelist.Count) temp++;
        }
        
        return id.GetPrice() * pricelist[temp];
    }
}