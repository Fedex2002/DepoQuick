using DataAccess.Context;
using DataAccess.Repository;
using Logic.DTOs;
using Logic.Interfaces;
using Model;

namespace Logic;

public class ApplicationController : IPromotionController
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
    
    public void UpdatePromotion(string promotion, Promotion newPromotion)
    {
        PromotionsRepository.UpdatePromotion(promotion, newPromotion);
    }
    
    public void DeletePromotion(Promotion promotion)
    {
        PromotionsRepository.DeletePromotion(promotion);
    }
    
    public List<PromotionDto> GetPromotionsDto()
    {
        List<Promotion> promotions = PromotionsRepository.GetAllPromotions();
        List<PromotionDto> promotionDtos = new List<PromotionDto>();
        
        foreach (Promotion promotion in promotions)
        {
            PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd);
            promotionDtos.Add(promotionDto);
        }
        
        return promotionDtos;
    }
    
    public PromotionDto GetPromotionDtoFromLabel(string label)
    {
        Promotion promotion = PromotionsRepository.FindPromotionByLabel(label);
        PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd);
        return promotionDto;
    }

    public Person CreatePerson(PersonDto personDto)
    {
        Person person = new Person(personDto.Name, personDto.Surname, personDto.Email, personDto.Password, personDto.IsAdmin);
        return person;
    }
}