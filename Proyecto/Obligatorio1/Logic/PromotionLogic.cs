using Repositories;
using Model;
namespace Logic;

public class PromotionLogic
{
    private PromotionsRepositories _promotionRepositories;
    
    public PromotionLogic(PromotionsRepositories promotionRepositories)
    {
        _promotionRepositories = promotionRepositories;
    }
    
    public Promotion ModifyPromotion(string label, int discount, DateTime startDate, DateTime endDate)
    {
        Promotion promotion = new Promotion(label, discount, startDate, endDate);
        return promotion;
    }
}