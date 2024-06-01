using Repositories;
using Logic;
using Logic.DTOs;
using Model;
using Model.Enums;
using Model.Exceptions;
using RepositoriesInterface;

namespace LogicTests;

[TestClass]
public class AdministratorLogicTests
{
    private BookingRepositories? _bookingRepo;
    private AdministratorLogic? _administratorLogic;
    private List<Booking>? _bookings;
    private Promotion? _promotion;
    private PromotionDto? _promotionDto;
    private List<PromotionDto>? _promotionsDto;
    private List<Promotion>? _promotions;
    private BookingDto? _bookingDto;
    private Booking? _booking;
    private Person _person;
    private PersonDto _personDto;
    private List<DateRange>? _availableDates;
    private List<DateRangeDto>? _availableDatesDto;
    private List<BookingDto> _bookingsDto;


    [TestInitialize]
    public void TestInitialize()
    {
        _bookingRepo = new BookingRepositories();
        _administratorLogic = new AdministratorLogic(_bookingRepo);
        _bookings = new List<Booking>();
        _promotions = new List<Promotion>();
        _promotionsDto = new List<PromotionDto>();
        _promotion = new Promotion("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotions.Add(_promotion);
        _availableDates = new List<DateRange>();
        _availableDates.Add(new DateRange(new DateTime(2024, 5, 24), new DateTime(2024, 5, 30)));
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnit("12", AreaType.A, SizeType.Small, true, _promotions, _availableDates), "", "Reservado",
            false,"johndoe@gmail.com");
        _bookingRepo.AddToRepository(_booking);
         _person= new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
        _promotionDto = new PromotionDto("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotionsDto.Add(_promotionDto);
        _availableDatesDto = new List<DateRangeDto>();
        _availableDatesDto.Add(new DateRangeDto(new DateTime(2024, 5, 24), new DateTime(2024, 5, 30)));
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", false,_person.Email);
        _bookingsDto = new List<BookingDto>();
        _bookingsDto.Add(_bookingDto);
        _personDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
    }

    [TestMethod]
    public void WhenAdministratorApprovesABookingDtoShouldChangeItToTrueAndToStatusCaptured()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", true, _person.Email);
        _administratorLogic.ApproveBooking(_personDto, _bookingDto);
        Assert.IsTrue(_bookingDto.Approved);
        Assert.AreEqual("Capturado", _bookingDto.Status);
    }
    
    [TestMethod]
    public void WhenAdministratorRejectsABookingDtoShouldWriteARejectionMessage()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", true,_person.Email);
        string rejectionMessage = "The booking has been rejected";
        _administratorLogic.SetRejectionMessage(_personDto, _bookingDto, rejectionMessage);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorRejectsABookingDtoWithEmptyMessageShouldThrowException()
    {
        _administratorLogic.SetRejectionMessage(_personDto, _bookingDto, "");
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyApprovedABookingShouldThrowException()
    {
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", true,_user.Email);
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyRejectedABookingShouldThrowException()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "Rejected",
            "Reservado", true,_user.Email);
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "Rejected");
    }
    
    [TestMethod] 
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyRejectedABookingAndTriesToApproveItShouldThrowException()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "Rejected",
            "Reservado", false,_user.Email);
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyApprovedABookingAndTriesToRejectItShouldThrowException()
    {
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", false,_user.Email);
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "Rejected");
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorTriesToApproveABookingAndUserDidNotMakeThePaymentShouldThrowException()
    {
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorTriesToRejectABookingAndUserDidNotMakeThePaymentShouldThrowException()
    {
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "Rejected");
    }
    
    [TestMethod]
    public void WhenGettingAllBookingsFromUsersShouldReturnThem()
    {
        List<Booking> bookings = _administratorLogic.GetAllUserBookings();
        Assert.IsTrue(bookings.Count > 0);
    
        
    }
    
    [TestMethod]
    
    public void WhenExportingToCsvShouldMakeTheFileInPath()
    {
        string projectRoot = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        string relativePath = @"Output\Bookings.csv";
        string filePath = Path.Combine(projectRoot, relativePath);
        _administratorLogic.ExportToCsv(filePath);
        Assert.IsTrue(File.Exists(filePath));
    }
}