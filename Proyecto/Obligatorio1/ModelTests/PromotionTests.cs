using Model;
namespace ModelTests;

[TestClass]
public class PromotionTests
{
    private Promotion _myPromotion;

    [TestInitialize]
    public void TestInitialize()
    {
        DateTime dateStart = new DateTime(2024,7,15);
        DateTime dateEnd = new DateTime(2024,10,15);
        _myPromotion = new Promotion("Descuento Invierno", 25, dateStart, dateEnd);
    }

    [TestMethod]
    public void CreatingEmptyPromotionShouldReturnEmpty()
    {
        Assert.IsNotNull(_myPromotion);
    }

    [TestMethod]
    public void CreatingPromotionWithLabelValidations_ShouldReturnTrueIfItIsAValidLabel()
    {
        Assert.IsTrue(_myPromotion.ValidateLabel());
    }

    [TestMethod]
    public void CreatingPromotionWithDiscountValidations_ShouldReturnTrueIfItIsAValidDiscount()
    {
        Assert.IsTrue(_myPromotion.ValidateDiscount());
    }
    
    [TestMethod]
    public void CreatingPromotionWithDateValidations_ShouldReturnTrueIfItIsAValidDate()
    {
        Assert.IsTrue(_myPromotion.ValidateDate());
    }
}