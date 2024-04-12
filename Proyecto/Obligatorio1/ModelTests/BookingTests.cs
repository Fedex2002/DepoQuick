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
}