using Model;

namespace ModelTests;

[TestClass]
public class DateRangeTests
{
    private DateRange _dateRange;
    private DateTime startDate;
    private DateTime endDate;
    
    [TestInitialize]
    public void TestInitialize()
    {
        startDate = new DateTime(2024,7,15);
        endDate = new DateTime(2024,10,15);
        _dateRange = new DateRange(startDate, endDate);
    }
    
    [TestMethod]
    public void CreatingEmptyDateRangeShouldReturnEmpty()
    {
        _dateRange = new DateRange();
        Assert.IsNotNull(_dateRange);
    }
    
    [TestMethod]
    public void CreatingADateRangeWithValues_ShouldReturnValues()
    {
        Assert.AreEqual(startDate, _dateRange.StartDate);
        Assert.AreEqual(endDate, _dateRange.EndDate);
    }

    [TestMethod]
    public void WhenDateIsInDateRangeShouldReturnTrue()
    {
        DateTime date = new DateTime(2024,8,15);
        Assert.IsTrue(_dateRange.Includes(date));
    }
}