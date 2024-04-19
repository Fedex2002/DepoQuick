using Model;

namespace Repositories;

public class PromotionsRepositories
{
    private List<Promotion> _promotions = new List<Promotion>();
    
    public void AddToRepository(Promotion promotion)
    {
        _promotions.Add(promotion);
    }
    public Promotion GetFromRepository(Promotion promotion)
    {
        return _promotions.Find(p => p.GetLabel() == promotion.GetLabel());
    }
    public bool ExistsInRepository(Promotion promotion)
    {
        return true;
    }
}