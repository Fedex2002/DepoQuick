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
}