using Model;
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

    [TestMethod]
    public void CreatingPromotionWithLabelValidations_ShouldReturnTrueIfItIsAValidLabel()
    {
        DateTime dateStart = new DateTime(2024,7,15);
        DateTime dateEnd = new DateTime(2024,10,15);
        _myPromotion = new Promotion("Descuento Invierno", 25, dateStart, dateEnd);
        Assert.IsTrue(_myPromotion.ValidateLabel());
    }
}