using Model;
using Repositories;

namespace RepositoriesTests;

[TestClass]

public class PromotionsRepositoriesTests
{
    private PromotionRepositories _promotionRepositories;
    private Promotion _promotion;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _promotionRepositories = new PromotionRepositories();
        _promotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
    }
    
    
    [TestMethod]
    public void WhenAddingNewPromotionShouldAddItToRepository()
    {
        _promotionRepositories.AddToRepository(_promotion);
        Promotion promotionInRepo = _promotionRepositories.GetFromRepository(_promotion);
        Assert.AreEqual(_promotion.GetLabel(), promotionInRepo.GetLabel());
    }
    
    [TestMethod]
    public void WhenAPromotionExistsInRepositoryShouldFindIt()
    {
        _promotionRepositories.AddToRepository(_promotion);
        Assert.IsTrue(_promotionRepositories.ExistsInRepository(_promotion.GetLabel()));
    }
    
    [TestMethod]
    public void WhenDeletingPromotionShouldRemoveItFromRepository()
    {
        _promotionRepositories.AddToRepository(_promotion);
        _promotionRepositories.RemoveFromRepository(_promotion);
        Assert.IsFalse(_promotionRepositories.ExistsInRepository(_promotion.GetLabel()));
    }
}