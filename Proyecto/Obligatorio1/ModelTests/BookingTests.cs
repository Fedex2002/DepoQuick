using Model;
namespace ModelTests;

[TestClass]
public class BookingTests
{
    private Booking _mybooking;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _mybooking = new Booking();
    }
    
    [TestMethod]
    public void CreatingEmptyBookingShouldReturnEmpty()
    {
        Assert.IsNotNull(_mybooking);
    }
    
    [TestMethod]
    public void CreatingBookingWithValidations_ShouldReturnValidValues()
    {
        _mybooking = new Booking(true, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        Assert.AreEqual(true, _mybooking.GetApproved());
        Assert.AreEqual(new DateTime(2024, 7, 15), _mybooking.GetDateStart());
        Assert.AreEqual(new DateTime(2024, 10, 15), _mybooking.GetDateEnd());
    }
}