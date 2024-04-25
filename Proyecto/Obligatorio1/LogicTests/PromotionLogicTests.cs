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
    
    [TestMethod]
    public void WhenModifyingPromotionShouldEliminateTheOldOneAndAddTheNewOne()
    {
        _promotionRepo.AddToRepository(_promotion);
        _promotionRepo.RemoveFromRepository(_promotion);
        Promotion newPromotion = _promotionLogic.ModifyPromotion("Summer discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionRepo.AddToRepository(newPromotion);
    }
}