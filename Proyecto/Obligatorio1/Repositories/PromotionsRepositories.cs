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
        return promotion;
    }
}