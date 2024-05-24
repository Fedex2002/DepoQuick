using Model;
using Model.Enums;
using Model.Exceptions;
namespace ModelTests;

[TestClass]
public class BookingTests
{
    private Booking _mybooking;
    private List<Promotion> _promotions;
    private Promotion _mypromotion;
    private StorageUnit _mystorageunit;
    
    [TestInitialize]
    public void TestInitialize()
    {
       _promotions = new List<Promotion>();
        _mypromotion= new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        _promotions.Add(_mypromotion);
        _mystorageunit= new StorageUnit("",AreaType.A, SizeType.Small, true, _promotions);
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, "Rejected", "Reservado", false);
    }
    
    [TestMethod]
    public void CreatingEmptyBookingShouldReturnEmpty()
    {
        _mybooking = new Booking();
        Assert.IsNotNull(_mybooking);
    }
    
    [TestMethod]
    public void CreatingBookingWithValidations_ShouldReturnValidValues()
    {
        Assert.AreEqual(false, _mybooking.Approved);
        Assert.AreEqual(new DateTime(2024, 7, 1), _mybooking.DateStart);
        Assert.AreEqual(_mystorageunit, _mybooking.StorageUnit);
        Assert.AreEqual(new DateTime(2024, 8, 15), _mybooking.DateEnd);
        Assert.AreEqual("Rejected", _mybooking.RejectedMessage);
        Assert.AreEqual("Reservado", _mybooking.Status);
        Assert.IsFalse(_mybooking.Payment);
    }
    
    [TestMethod]
    public void CreatingBookingWithDateStartAndDayEnd_ShouldReturnCountOfDaysOfBooking()
    {
        Assert.AreEqual(45, _mybooking.GetCountOfDays());
    }

    [TestMethod]
    public void CalculatingBookingTotalPriceWithValidations_ShouldReturnTotalPrice()
    {
        Assert.AreEqual(2126.25, _mybooking.CalculateBookingTotalPrice());
       _mystorageunit= new StorageUnit("",AreaType.A, SizeType.Small, true, _promotions);
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 7, 4), _mystorageunit, "Rejected", "Reservado", false);
        Assert.AreEqual(157.5, _mybooking.CalculateBookingTotalPrice());
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 7, 9), _mystorageunit, "Rejected", "Reservado", false);
        Assert.AreEqual(399, _mybooking.CalculateBookingTotalPrice());
    }

    [TestMethod]
    public void WhenRejectingBookingWithValidations_ShouldReturnTrueIfValid()
    {
        Assert.AreEqual(true, _mybooking.CheckRejection());
    }

    [TestMethod]
    [ExpectedException(typeof(BookingExceptions))]
    public void WhenRejectingBookingWithValidations_ShouldReturnExceptionIfNotValid()
    {
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, 
            "Lamentamos informarte que, después de una revisión exhaustiva y consideración cuidadosa, hemos decidido que no podremos avanzar con tu solicitud en esta ocasión. Nos gustaría expresarte nuestro agradecimiento por haber compartido tu propuesta con nosotros y por tu interés en colaborar con nuestro equipo. Valoramos sinceramente el tiempo y el esfuerzo que has dedicado a esta oportunidad. Por favor, no dudes en ponerte en contacto con nosotros si tienes alguna pregunta o si deseas obtener más información sobre nuestra decisión. Te deseamos todo lo mejor en tus futuros esfuerzos y proyectos!.", "Reservado", false);
    }
    
    [TestMethod]
    public void CreatingBookingWithDateValidations_ShouldReturnTrueIfValid()
    {
        Assert.AreEqual(true, _mybooking.CheckDate());
    }
    
    [TestMethod]
    [ExpectedException(typeof(BookingExceptions))]
    public void CreatingBookingWithInvalidDate_ShouldReturnException()
    {
        _mybooking = new Booking(false, new DateTime(2024, 5, 15), new DateTime(2024, 5, 14), _mystorageunit, "", "Reservado", false);
    }
    
    [TestMethod]
    public void WhenCreatingBookingStatusShouldBePending()
    {
        Assert.AreEqual("Reservado", _mybooking.Status);
    }
    
    [TestMethod]
    public void WhenCreatingBookingPaymentShouldBeFalse()
    {
        Assert.IsFalse(_mybooking.Payment);
    }
    
    [TestMethod]
    public void WhenSettingBookingStatusToOkShouldSetIt()
    {
       _mybooking.Status = "Capturado";
        Assert.AreEqual("Capturado", _mybooking.Status);
    }
    
    [TestMethod]
    public void WhenSettingPaymentToTrueShouldSetIt()
    {
       _mybooking.Approved = true;
        Assert.IsTrue(_mybooking.Approved);
    }
}