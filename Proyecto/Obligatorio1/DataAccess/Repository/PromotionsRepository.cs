using DataAccess.Context;
using Model;

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
        AddNewPromotionToPromotionsTable(promotion);

    }
    private void AddNewPromotionToPromotionsTable(Promotion promotion)
    {
        _database.Promotions.Add(promotion);

        _database.SaveChanges();
    }
}

