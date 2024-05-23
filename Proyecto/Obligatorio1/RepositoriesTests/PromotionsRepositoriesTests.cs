using Model;
using Repositories;

namespace RepositoriesTests;

[TestClass]

public class PromotionsRepositoriesTests
{
    private PromotionsRepositories _promotionRepositories;
    private Promotion _promotion;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _promotionRepositories = new PromotionsRepositories();
        _promotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
    }
    
    
    [TestMethod]
    public void WhenAddingNewPromotionShouldAddItToRepository()
    {
        _promotionRepositories.AddToRepository(_promotion);
        Promotion promotionInRepo = _promotionRepositories.GetFromRepository(_promotion.Label);
        Assert.AreEqual(_promotion.Label, promotionInRepo.Label);
    }
    
    [TestMethod]
    public void WhenAPromotionExistsInRepositoryShouldFindIt()
    {
        _promotionRepositories.AddToRepository(_promotion);
        Assert.IsTrue(_promotionRepositories.ExistsInRepository(_promotion.Label));
    }
    
    [TestMethod]
    public void WhenDeletingPromotionShouldRemoveItFromRepository()
    {
        _promotionRepositories.AddToRepository(_promotion);
        _promotionRepositories.RemoveFromRepository(_promotion);
        Assert.IsFalse(_promotionRepositories.ExistsInRepository(_promotion.Label));
    }
    
    [TestMethod]
    
    public void WhenGettingAllPromotionsFromRepositoryShouldReturnIt()
    {
           Promotion promotion2 = new Promotion("Descuento Verano", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            _promotionRepositories.AddToRepository(_promotion);
            _promotionRepositories.AddToRepository(promotion2);
            List<Promotion> promotions = _promotionRepositories.GetAllFromRepository();
            Assert.AreEqual(2, promotions.Count);
    }
}