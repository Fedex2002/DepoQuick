using Model;
using Model.Enums;
namespace ModelTests;

[TestClass]
public class BookingTests
{
    private Booking _mybooking;
    
    [TestInitialize]
    public void TestInitialize()
    {
        List<Promotion> p = new List<Promotion>();
        Promotion myPromotion = new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        p.Add(myPromotion);
        StorageUnit storageUnitSmall = new StorageUnit(AreaType.A, SizeType.Small, true, p);
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), storageUnitSmall);
    }
    
    [TestMethod]
    public void CreatingEmptyBookingShouldReturnEmpty()
    {
        _mybooking = new Booking();
        Assert.IsNotNull(_mybooking);
    }
    
    [TestMethod]
    public void CreatingBookingWithValidations_ShouldReturnValidValues()
    {
        Assert.AreEqual(true, _mybooking.GetApproved());
        Assert.AreEqual(new DateTime(2024, 7, 1), _mybooking.GetDateStart());
        Assert.AreEqual(new DateTime(2024, 8, 15), _mybooking.GetDateEnd());
    }
    
    [TestMethod]
    public void CreatingBookingWithDateStartAndDayEnd_ShouldReturnCountOfDaysOfBooking()
    {
        Assert.AreEqual(45, _mybooking.GetCountOfDays());
    }

    [TestMethod]
    public void CalculatingBookingTotalPriceWithValidations_ShouldReturnTotalPrice()
    {
        Assert.AreEqual(52.5, _mybooking.CalculateBookingTotalPrice());
    }
    
}