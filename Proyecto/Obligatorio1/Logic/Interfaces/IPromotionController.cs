using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IPromotionController
{
    public Promotion CreatePromotion(PromotionDto promotionDto);
    public void AddPromotion(Promotion promotion);
    public void UpdatePromotion(Promotion promotion, PromotionDto promotionDto);
    public void DeletePromotion(Promotion promotion);
    public List<PromotionDto> GetPromotionsDto();
    public PromotionDto GetPromotionDtoFromLabel(string label);
    
}