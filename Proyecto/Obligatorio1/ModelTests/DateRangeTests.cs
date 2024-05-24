using Model;

namespace ModelTests;

[TestClass]
public class DateRangeTests
{
    private DateRange _dateRange;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _dateRange = new DateRange();
    }
    
    [TestMethod]
    public void CreatingEmptyDateRangeShouldReturnEmpty()
    {
        Assert.IsNotNull(_dateRange);
    }
    
    [TestMethod]
    public void CreatingADateRangeWithValues_ShouldReturnValues()
    {
        DateTime startDate = new DateTime(2024,7,15);
        DateTime endDate = new DateTime(2024,10,15);
        _dateRange = new DateRange(startDate, endDate);
        Assert.AreEqual(startDate, _dateRange.StartDate);
        Assert.AreEqual(endDate, _dateRange.EndDate);
    }
}