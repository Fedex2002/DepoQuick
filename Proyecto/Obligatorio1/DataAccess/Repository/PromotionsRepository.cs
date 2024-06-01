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
    
    public void UpdatePromotion(Promotion promotion)
    {
        Promotion dbPromotion = FindPromotionByLabel(promotion.Label);
        if (dbPromotion != null)
        {
            dbPromotion.Label = promotion.Label;
            dbPromotion.Discount = promotion.Discount;
            dbPromotion.DateStart = promotion.DateStart;
            dbPromotion.DateEnd = promotion.DateEnd;
            _database.SaveChanges();
        }
    }
    
    public Promotion FindPromotionByLabel(string label)
    {
        Promotion promotion = _database.Promotions.FirstOrDefault(prom => prom.Label == label);
        return promotion;
    }
}

