using Model;
using Repositories;

namespace RepositoriesTests;

[TestClass]

public class PromotionsRepositoriesTests
{
    private PromotionsRepositories _promotionsRepositories;
    private Promotion _promotion;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _promotionsRepositories = new PromotionsRepositories();
        _promotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
    }
    
    
    [TestMethod]
    public void WhenAddingNewPromotionShouldAddItToRepository()
    {
        _promotionsRepositories.AddToRepository(_promotion);
        Promotion promotionInRepo = _promotionsRepositories.GetFromRepository(_promotion);
        Assert.AreEqual(_promotion.GetLabel(), promotionInRepo.GetLabel());
    }
    
    [TestMethod]
    public void WhenAPromotionExistsInRepositoryShouldFindIt()
    {
        _promotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionsRepositories.AddToRepository(_promotion);
        Assert.IsTrue(_promotionsRepositories.ExistsInRepository(_promotion));
    }
}