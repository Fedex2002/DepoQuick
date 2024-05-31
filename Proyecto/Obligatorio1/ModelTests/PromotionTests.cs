using Model;
using Model.Exceptions;
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
        _myPromotion = new Promotion();
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
    
    [TestMethod]
    public void CreatingPromotionWithDiscount_ShouldReturnAValue()
    {
        _myPromotion = new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        Assert.AreEqual(25, _myPromotion.Discount);
    }

    [TestMethod]
    public void ModifyingAPromotionWithNewData_ShouldReturnValues()
    {
        _myPromotion = new Promotion("Descuento Verano", 50, new DateTime(2024,1,3), new DateTime(2024,2,24));
        Assert.AreEqual("Descuento Verano", _myPromotion.Label);
        Assert.AreEqual(50, _myPromotion.Discount);
        Assert.AreEqual(new DateTime(2024,1,3), _myPromotion.DateStart);
        Assert.AreEqual(new DateTime(2024,2,24), _myPromotion.DateEnd);
    }
    
    [TestMethod]
    [ExpectedException(typeof(PromotionExceptions))]
    public void CreatingPromotionWithInvalidLabel_ShouldReturnException()
    {
        _myPromotion = new Promotion("Descuento Verano 2024", 50, new DateTime(2024,1,3), new DateTime(2024,2,24));
    }
    
    [TestMethod]
    [ExpectedException(typeof(PromotionExceptions))]
    public void CreatingPromotionWithInvalidDiscount_ShouldReturnException()
    {
        _myPromotion = new Promotion("Descuento Verano", 80, new DateTime(2024,1,3), new DateTime(2024,2,24));
    }
    
    [TestMethod]
    [ExpectedException(typeof(PromotionExceptions))]
    public void CreatingPromotionWithInvalidDate_ShouldReturnException()
    {
        _myPromotion = new Promotion("Descuento Verano", 50, new DateTime(2024,1,3), new DateTime(2024,1,1));
    }
}