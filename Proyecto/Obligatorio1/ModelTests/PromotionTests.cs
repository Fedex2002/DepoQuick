namespace ModelTests;

[TestClass]
public class PromotionTests
{
    private Promotion _myPromotion;

    [TestInitialize]
    public void TestInitialize()
    {
        _myPromotion = new Promotion();
    }

    [TestMethod]
    public void CreatingEmptyPromotionShouldReturnEmpty()
    {
        Assert.IsNotNull(_myPromotion);
    }
}