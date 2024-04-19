namespace RepositoriesTests;

[TestClass]

public class PromotionsRepositoriesTests
{
    private PromotionsRepositories _promotionsRepositories;
    
    
    [TestMethod]
    public void WhenAddingNewPromotionShouldAddItToRepository()
    {
        Promotion promotion = new Promotion();
        _promotionsRepositories.AddToRepository(promotion);
        Promotion promotionInRepo = _promotionsRepositories.GetFromRepository(promotion);
        Assert.AreEqual(promotion.GetName(), promotionInRepo.GetName());
    }
}