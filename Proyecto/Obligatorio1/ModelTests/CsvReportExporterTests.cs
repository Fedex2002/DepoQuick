using Model;
using Model.Enums;

namespace ModelTests;
[TestClass]
public class CsvReportExporterTests
{
    private Booking _mybooking;
    private CsvReportExporter _csvReportExporter;
    private List<Promotion> _promotions;
    private Promotion _mypromotion;
    private StorageUnit _mystorageunit;
    private List<DateRange> _availableDates;
    
    [TestInitialize]
    void TestInitialize()
    {
        _csvReportExporter = new CsvReportExporter();
        _promotions = new List<Promotion>();
        _mypromotion= new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        _promotions.Add(_mypromotion);
        _mystorageunit= new StorageUnit("",AreaType.A, SizeType.Small, true, _promotions, _availableDates);
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, "Rejected", "Reservado", false);
    }
    
    [TestMethod]
    public void CreatingEmptyCsvReportExporterShouldReturnEmpty()
    {
        _csvReportExporter = new CsvReportExporter();
        Assert.IsNotNull(_csvReportExporter);
    }
}