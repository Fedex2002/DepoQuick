using Controllers.Dtos;
using Controllers.Interfaces;
using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Exceptions;

namespace Controllers;

public class PromotionController : IPromotionController
{
    private readonly PromotionsRepository _promotionRepositories;
    
    public PromotionController(ApplicationDbContext context)
    {
        _promotionRepositories = new PromotionsRepository(context);
    }
    
    public void CreatePromotion(PromotionDto promotionDto)
    {
        Promotion promotion= new Promotion(promotionDto.Label,promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd); 
        _promotionRepositories.AddPromotion(promotion);
        
    }
    
    public void ModifyPromotion(string oldLabel, PromotionDto newPromotionDto)
    {
        if (!_promotionRepositories.PromotionAlreadyExists(oldLabel))
        {
            IfPromotionDoesNotExistThrowException();
        }
        else
        {
            Promotion newPromotion= new Promotion(newPromotionDto.Label, newPromotionDto.Discount, newPromotionDto.DateStart, newPromotionDto.DateEnd);
            _promotionRepositories.UpdatePromotion(oldLabel, newPromotion);
        }
    }
    
    private static void IfPromotionDoesNotExistThrowException()
    {
        throw new LogicExceptions("Promotion does not exist");
    }
    
    public void RemovePromotion(PromotionDto promotionDto)
    {
        Promotion promotionInRepo= _promotionRepositories.FindPromotionByLabel(promotionDto.Label);
        if (!_promotionRepositories.PromotionAlreadyExists(promotionDto.Label))
        {
            IfPromotionDoesNotExistThrowException();
        }
        else
        {
            _promotionRepositories.DeletePromotion(promotionInRepo);
        }
    }
    
    public List<PromotionDto> GetPromotionsDto()
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach (var promotion in _promotionRepositories.GetAllPromotions())
        {
            promotionsDto.Add(new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd));
        }
        
        return promotionsDto;
    }
    
    public PromotionDto GetPromotionDtoFromLabel(string label)
    {
        Promotion promotion= _promotionRepositories.FindPromotionByLabel(label);
        PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd);
        return promotionDto;
    }
}