using Repositories;
using Logic;
using Logic.DTOs;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class AdministratorLogicTests
{
    private PersonRepositories? _personRepo;
    private AdministratorLogic? _administratorLogic;
    private List<Booking>? _bookings;
    private Promotion? _promotion;
    private PromotionDto? _promotionDto;
    private List<PromotionDto>? _promotionsDto;
    private List<Promotion>? _promotions;
    private BookingDto? _bookingDto;
    private Booking? _booking;
    private UserDto? _userDto;
    private User? _user;
    private List<DateRange>? _availableDates;
    private List<DateRangeDto>? _availableDatesDto;
    private List<BookingDto> _bookingsDto;


    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _administratorLogic = new AdministratorLogic(_personRepo);
        _bookings = new List<Booking>();
        _promotions = new List<Promotion>();
        _promotionsDto = new List<PromotionDto>();
        _promotion = new Promotion("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotions.Add(_promotion);
        _availableDates = new List<DateRange>();
        _availableDates.Add(new DateRange(new DateTime(2024, 5, 24), new DateTime(2024, 5, 30)));
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnit("12", AreaType.A, SizeType.Small, true, _promotions, _availableDates), "", "Reservado",
            false);
        _bookings.Add(_booking);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookings);
        _personRepo.AddToRepository(_user);
        _promotionDto = new PromotionDto("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotionsDto.Add(_promotionDto);
        _availableDatesDto = new List<DateRangeDto>();
        _availableDatesDto.Add(new DateRangeDto(new DateTime(2024, 5, 24), new DateTime(2024, 5, 30)));
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", false);
        _bookingsDto = new List<BookingDto>();
        _bookingsDto.Add(_bookingDto);
        _userDto = new UserDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookingsDto);
    }

    [TestMethod]
    public void WhenAdministratorApprovesABookingDtoShouldChangeItToTrueAndToStatusCaptured()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", true);
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }

    [TestMethod]
    public void WhenAdministratorRejectsABookingDtoShouldWriteARejectionMessage()
    {
        string rejectionMessage = "The booking has been rejected";
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, rejectionMessage);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorRejectsABookingDtoWithEmptyMessageShouldThrowException()
    {
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "");
    }

    [TestMethod]
    public void WhenAdministratorIsTryingToApproveOrRejectBookingsShouldGetAListOfUsersDto()
    {
        List<UserDto> users = _administratorLogic.GetUsersDto();
        Assert.IsTrue(users.Count > 0);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyApprovedABookingShouldThrowException()
    {
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", false);
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyRejectedABookingShouldThrowException()
    {
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "Rejected");
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "Rejected",
            "Reservado", false);
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "Rejected");
    }

    [TestMethod] 
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyRejectedABookingAndTriesToApproveItShouldThrowException()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "Rejected",
            "Reservado", false);
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyApprovedABookingAndTriesToRejectItShouldThrowException()
    {
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", false);
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "Rejected");
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorTriesToApproveABookingAndUserDidNotMakeThePaymentShouldThrowException()
    {
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }
}