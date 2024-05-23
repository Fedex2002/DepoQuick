using RepositoriesInterface;
using Model;

namespace Repositories;

public class PromotionsRepositories : IRepositories<Promotion>
{
    private readonly List<Promotion> _promotions = new List<Promotion>();
    
    public void AddToRepository(Promotion promotion)
    {
        _promotions.Add(promotion);
    }
    public Promotion GetFromRepository(string label)
    {
        return _promotions.Find(p => p.GetLabel() == label);
    }
    public bool ExistsInRepository(string label)
    {
        return _promotions.Any(p => p.GetLabel() == label);
    }
    public void RemoveFromRepository(Promotion promotion)
    {
        _promotions.Remove(promotion);
    }
    public List<Promotion> GetAllFromRepository()
    {
        return _promotions;
    }
}