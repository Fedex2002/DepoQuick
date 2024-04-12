using Model;
namespace ModelTests;

[TestClass]
public class BookingTests
{
    private Booking _mybooking;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15));
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
    
}