using Model;
using Model.Enums;

namespace ModelTests;

[TestClass]
public class ReportExporterTests
{
    private Booking _mybooking;
    private List<Promotion> _promotions;
    private Promotion _mypromotion;
    private StorageUnit _mystorageunit;
    private List<DateRange> _availableDates;
    private List<Booking> _bookings;

    [TestInitialize]
    public void TestInitialize()
    {
        _promotions = new List<Promotion>();
        _bookings = new List<Booking>();
        _availableDates = new List<DateRange>();
        _mypromotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15),
            new DateTime(2024, 10, 15));
        _promotions.Add(_mypromotion);
        _mystorageunit = new StorageUnit("", AreaType.A, SizeType.Small, true, _promotions, _availableDates);
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit,
            "Rejected", "Reservado", false, "samplemail@gmail.com");
        _bookings.Add(_mybooking);
    }

    [TestMethod]
    public void WhenCreatingCsvReportExporterShouldReturnCorrectInstance()
    {
        var exporter = ReportExporter.Create("csv");
        Assert.IsInstanceOfType(exporter, typeof(CsvReportExporter));
    }
    
    [TestMethod]

    public void WhenCreatingTxtReportExporterShouldReturnCorrectInstance()
    {
        var exporter = ReportExporter.Create("txt");
        Assert.IsInstanceOfType(exporter, typeof(TxtReportExporter));
    }
}