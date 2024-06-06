using DataAccess.Context;
using DataAccess.Repository;
using Logic.DTOs;
using Logic.Interfaces;
using Repositories;
using Model;
using Model.Exceptions;

namespace Logic;

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
        if (_promotionRepositories.PromotionAlreadyExists(promotionDto.Label))
        {
            IfPromotionExistsThrowException();
        }
        else
        {
            _promotionRepositories.AddPromotion(promotion);
        }
    }
    
    private static void IfPromotionExistsThrowException()
    {
        throw new LogicExceptions("Promotion already exists");
    }
    
    public void ModifyPromotion(string oldLabel, PromotionDto newPromotionDto)
    {
        Promotion promotionInRepo= _promotionRepositories.FindPromotionByLabel(oldLabel);
        if (!_promotionRepositories.PromotionAlreadyExists(oldLabel))
        {
            IfPromotionDoesNotExistThrowException();
        }
        else
        {
            EditPromotion(newPromotionDto, promotionInRepo);
        }
    }
    
    private static void IfPromotionDoesNotExistThrowException()
    {
        throw new LogicExceptions("Promotion does not exist");
    }

    private static void EditPromotion(PromotionDto promotionDto, Promotion promotionInRepo)
    {
        promotionInRepo.Label = promotionDto.Label;
        promotionInRepo.Discount = promotionDto.Discount;
        promotionInRepo.DateStart = promotionDto.DateStart;
        promotionInRepo.DateEnd = promotionDto.DateEnd;
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