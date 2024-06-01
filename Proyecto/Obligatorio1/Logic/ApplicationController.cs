using DataAccess.Context;
using DataAccess.Repository;
using Logic.DTOs;
using Model;

namespace Logic;

public class ApplicationController
{
    public PromotionsRepository PromotionsRepository;
    
    public ApplicationController(ApplicationDbContext context)
    {
        PromotionsRepository = new PromotionsRepository(context);
    }
    
    public Promotion CreatePromotion(PromotionDto promotionDto)
    {
        Promotion promotion = new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd);
        return promotion;
    }
    
    public void AddPromotion(Promotion promotion)
    {
        PromotionsRepository.AddPromotion(promotion);
    }
    
    public void UpdatePromotion(Promotion promotion, Promotion newPromotion)
    {
        PromotionsRepository.UpdatePromotion(promotion, newPromotion);
    }
    
    public void DeletePromotion(Promotion promotion)
    {
        PromotionsRepository.DeletePromotion(promotion);
    }
}