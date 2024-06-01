using DataAccess.Context;
using Model;
using Model.Exceptions;

namespace DataAccess.Repository;

public class PromotionsRepository
{
    private ApplicationDbContext _database;
    
    public PromotionsRepository(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void AddPromotion(Promotion promotion)
    {
        if (_database.Promotions.Any(promotion => promotion.Label == promotion.Label))
        {
            throw new RepositoryExceptions("The promotion already exists");
        }
        
        AddNewPromotionToPromotionsTable(promotion);
    }
    private void AddNewPromotionToPromotionsTable(Promotion promotion)
    {
        _database.Promotions.Add(promotion);

        _database.SaveChanges();
    }
}

