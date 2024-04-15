using Model;
using Model.Enums;
namespace ModelTests;

[TestClass]
public class BookingTests
{
    private Booking _mybooking;
    private List<Promotion> _promotions;
    private Promotion _mypromotion;
    private StorageUnit _mystorageunit;
    
    [TestInitialize]
    public void TestInitialize()
    {
       _promotions = new List<Promotion>();
        _mypromotion= new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        _promotions.Add(_mypromotion);
        _mystorageunit= new StorageUnit(AreaType.A, SizeType.Small, true, _promotions);
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, "Rejected");
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
        Assert.AreEqual("Rejected", _mybooking.GetRejectedBooking());
    }
    
    [TestMethod]
    public void CreatingBookingWithDateStartAndDayEnd_ShouldReturnCountOfDaysOfBooking()
    {
        Assert.AreEqual(45, _mybooking.GetCountOfDays());
    }

    [TestMethod]
    public void CalculatingBookingTotalPriceWithValidations_ShouldReturnTotalPrice()
    {
        Assert.AreEqual(2126.25, _mybooking.CalculateBookingTotalPrice());
       _mystorageunit= new StorageUnit(AreaType.A, SizeType.Small, true, _promotions);
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 7, 4), _mystorageunit, "Rejected");
        Assert.AreEqual(157.5, _mybooking.CalculateBookingTotalPrice());
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 7, 9), _mystorageunit, "Rejected");
        Assert.AreEqual(399, _mybooking.CalculateBookingTotalPrice());
    }

}