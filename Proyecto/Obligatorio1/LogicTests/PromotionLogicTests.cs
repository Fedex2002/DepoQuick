using System.Runtime.InteropServices.ComTypes;
using Repositories;
using Model;
using Logic;
namespace LogicTests;

[TestClass]
public class PromotionLogicTests
{
    private PromotionsRepositories _promotionRepo;
    private PromotionLogic _promotionLogic;
    private Promotion _promotion;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _promotionRepo = new PromotionsRepositories();
        _promotionLogic = new PromotionLogic(_promotionRepo);
        _promotion = new Promotion("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
    }
    
    [TestMethod] public void WhenModifyingPromotionShouldEliminateTheOldOneAndAddTheNewOne()
    {
        _promotionRepo.AddToRepository(_promotion);
                        
        _promotionDto= new PromotionDTO("Summer discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionLogic.ModifyPromotion(_promotionDto);
        Assert.AreEqual(_promotionDto.Label, _promotionRepo.GetFromRepository(_promotionDto.Label).GetLabel());
        Assert.AreEqual(_promotionDto.Discount, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDiscount());
        Assert.AreEqual(_promotionDto.DateStart, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDateStart());
        Assert.AreEqual(_promotionDto.DateEnd, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDateEnd());
                        
                        
    }
}