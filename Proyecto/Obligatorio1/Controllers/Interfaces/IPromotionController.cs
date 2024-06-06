using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IPromotionController
{
    public void CreatePromotion(PromotionDto promotionDto);
    public void ModifyPromotion(string label, PromotionDto newPromotionDto);
    public void RemovePromotion(PromotionDto promotionDto);
    public List<PromotionDto> GetPromotionsDto();
    public PromotionDto GetPromotionDtoFromLabel(string label);
    
}