using Model;
using Repositories;

namespace RepositoriesTests;

[TestClass]

public class PromotionsRepositoriesTests
{
    private PromotionsRepositories _promotionsRepositories;
    private Promotion _promotion;
    
    [TestMethod]
    public void WhenAddingNewPromotionShouldAddItToRepository()
    {
        _promotionsRepositories = new PromotionsRepositories(); 
        _promotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionsRepositories.AddToRepository(_promotion);
        Promotion promotionInRepo = _promotionsRepositories.GetFromRepository(_promotion);
        Assert.AreEqual(_promotion.GetLabel(), promotionInRepo.GetLabel());
    }
}