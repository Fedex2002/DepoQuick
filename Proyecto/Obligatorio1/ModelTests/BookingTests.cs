namespace ModelTests;

[TestClass]
public class BookingTests
{
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