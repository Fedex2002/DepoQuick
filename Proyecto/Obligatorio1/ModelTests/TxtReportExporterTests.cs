using Model;
using Model.Enums;

namespace ModelTests
{
    [TestClass]
    public class TxtReportExporterTests
    {
        private Booking _mybooking;
        private TxtReportExporter _txtReportExporter;
        private List<Promotion> _promotions;
        private Promotion _mypromotion;
        private StorageUnit _mystorageunit;
        private List<DateRange> _availableDates;
        private List<Booking> _bookings;

        [TestInitialize]
        public void TestInitialize()
        {
            _txtReportExporter = new TxtReportExporter();
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
        public void CreatingEmptyTxtReportExporterShouldReturnEmpty()
        {
            _txtReportExporter = new TxtReportExporter();
            Assert.IsNotNull(_txtReportExporter);
        }
        
        [TestMethod]
        public void WhenExportingAsTxtShouldReturnCorrectTxtString()
        {
            string expectedData = "StorageUnit Id: \r\nArea: A\r\nSize: Small\r\nClimatization: True\r\nStartDate: 2024-07-01\r\nEndDate: 2024-08-15\r\nStatus: Reservado\r\n\r\n";
            string actualData = _txtReportExporter.Export(_bookings);
            Assert.AreEqual(expectedData, actualData);
        }
        
        [TestMethod]
        public void WhenGettingDataFromBookingsShouldReturnIt()
        {
            string expectedData = "StorageUnit Id: \r\nArea: A\r\nSize: Small\r\nClimatization: True\r\nStartDate: 2024-07-01\r\nEndDate: 2024-08-15\r\nStatus: Reservado\r\n\r\n";
            Assert.AreEqual(expectedData, _txtReportExporter.GetData(_bookings));
        }
    }
}