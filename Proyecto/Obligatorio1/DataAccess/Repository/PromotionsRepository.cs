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
        if (PromotionAlreadyExists(promotion))
        {
            PromotionAlreadyExistsSoThrowException();
        }
        
        AddNewPromotionToPromotionsTable(promotion);
    }

    private static void PromotionAlreadyExistsSoThrowException()
    {
        throw new RepositoryExceptions("The promotion already exists");
    }

    private bool PromotionAlreadyExists(Promotion newPromotion)
    {
        return _database.Promotions.Any(promotion => promotion.Label == newPromotion.Label);
    }

    private void AddNewPromotionToPromotionsTable(Promotion promotion)
    {
        _database.Promotions.Add(promotion);

        _database.SaveChanges();
    }
}

