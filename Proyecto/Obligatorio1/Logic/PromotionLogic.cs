using Logic.DTOs;
using Repositories;
using Model;
using Model.Exceptions;

namespace Logic;

public class PromotionLogic
{
    private PromotionsRepositories _promotionRepositories;
    
    public PromotionLogic(PromotionsRepositories promotionRepositories)
    {
        _promotionRepositories = promotionRepositories;
    }
    
    public void ModifyPromotion(PromotionDto promotionDto)
    {
        Promotion promotionInRepo= _promotionRepositories.GetFromRepository(promotionDto.Label);
        if (_promotionRepositories.GetFromRepository(promotionDto.Label) == null)
        {
            throw new LogicExceptions("Promotion does not exist");
        }
        else
        {
            _promotionRepositories.RemoveFromRepository(promotionInRepo);
            promotionInRepo= new Promotion(promotionDto.Label,promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd);
            _promotionRepositories.AddToRepository(promotionInRepo);
        }
    }
    
    public void CreatePromotion(PromotionDto promotionDto)
    {
        Promotion promotion= new Promotion(promotionDto.Label,promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd);
        if (_promotionRepositories.GetFromRepository(promotionDto.Label) != null)
        {
            throw new LogicExceptions("Promotion already exists");
        }
        else
        {
            _promotionRepositories.AddToRepository(promotion);
        }
    }
    
    public void RemovePromotion(PromotionDto promotionDto)
    {
        Promotion promotionInRepo= _promotionRepositories.GetFromRepository(promotionDto.Label);
        if (_promotionRepositories.GetFromRepository(promotionDto.Label) == null)
        {
            throw new LogicExceptions("Promotion does not exist");
        }
        else
        {
            _promotionRepositories.RemoveFromRepository(promotionInRepo);
        }
    }
    
    public List<PromotionDto> GetPromotionsDto()
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        PromotionDto promotionDto = new PromotionDto("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15)); 
        promotionsDto.Add(promotionDto);
        return promotionsDto;
    }
}