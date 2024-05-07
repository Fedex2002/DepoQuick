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
    
    public void ModifyPromotion(PromotionDto promotionDto, string oldLabel)
    {
        Promotion promotionInRepo= _promotionRepositories.GetFromRepository(oldLabel);
        if (_promotionRepositories.GetFromRepository(oldLabel) == null)
        {
            IfPromotionDoesNotExistThrowException();
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
            IfPromotionExistsThrowException();
        }
        else
        {
            _promotionRepositories.AddToRepository(promotion);
        }
    }

    private static void IfPromotionExistsThrowException()
    {
        throw new LogicExceptions("Promotion already exists");
    }

    public void RemovePromotion(PromotionDto promotionDto)
    {
        Promotion promotionInRepo= _promotionRepositories.GetFromRepository(promotionDto.Label);
        if (_promotionRepositories.GetFromRepository(promotionDto.Label) == null)
        {
            IfPromotionDoesNotExistThrowException();
        }
        else
        {
            _promotionRepositories.RemoveFromRepository(promotionInRepo);
        }
    }

    private static void IfPromotionDoesNotExistThrowException()
    {
        throw new LogicExceptions("Promotion does not exist");
    }

    public List<PromotionDto> GetPromotionsDto()
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach (var promotion in _promotionRepositories.GetAllFromRepository())
        {
            promotionsDto.Add(new PromotionDto(promotion.GetLabel(), promotion.GetDiscount(), promotion.GetDateStart(), promotion.GetDateEnd()));
        }
        
        return promotionsDto;
    }
    
    public PromotionDto GetPromotionDtoFromLabel(string label)
    {
        Promotion promotion= _promotionRepositories.GetFromRepository(label);
        PromotionDto promotionDto = new PromotionDto(promotion.GetLabel(), promotion.GetDiscount(), promotion.GetDateStart(), promotion.GetDateEnd());
        return promotionDto;
    }
}