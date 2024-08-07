using Model;
using Model.Enums;

namespace ModelTests
{
    [TestClass]
    public class CsvReportExporterTests
    {
        private Booking _mybooking;
        private CsvReportExporter _csvReportExporter;
        private List<Promotion> _promotions;
        private Promotion _mypromotion;
        private StorageUnit _mystorageunit;
        private List<DateRange> _availableDates;
        private List<Booking> _bookings;

        [TestInitialize]
        public void TestInitialize()
        {
            _csvReportExporter = new CsvReportExporter();
            _promotions = new List<Promotion>();
            _bookings = new List<Booking>();
            _availableDates = new List<DateRange>();
            _mypromotion = new Promotion("Descuento Invierno", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            _promotions.Add(_mypromotion);
            _mystorageunit = new StorageUnit("", AreaType.A, SizeType.Small, true, _promotions, _availableDates);
            _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, "Rejected", "Reservado", false, "samplemail@gmail.com");
            _bookings.Add(_mybooking);
        }

        [TestMethod]
        public void CreatingEmptyCsvReportExporterShouldReturnEmpty()
        {
            _csvReportExporter = new CsvReportExporter();
            Assert.IsNotNull(_csvReportExporter);
        }

        [TestMethod]
        public void WhenExportingAsCsvShouldReturnCorrectCsvString()
        {
            string expectedData = $"StorageUnit Id,Area,Size,Climatization,StartDate,EndDate,Status{Environment.NewLine}\"\",\"A\",\"Small\",\"True\",\"2024-07-01\",\"2024-08-15\",\"Reservado\"{Environment.NewLine}";
            string actualData = _csvReportExporter.Export(_bookings);
            Assert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void WhenGettingDataFromBookingsShouldReturnIt()
        {
            string expectedData = $"StorageUnit Id,Area,Size,Climatization,StartDate,EndDate,Status{Environment.NewLine}\"\",\"A\",\"Small\",\"True\",\"2024-07-01\",\"2024-08-15\",\"Reservado\"{Environment.NewLine}";
            Assert.AreEqual(expectedData, _csvReportExporter.GetData(_bookings));
        }
    }
}