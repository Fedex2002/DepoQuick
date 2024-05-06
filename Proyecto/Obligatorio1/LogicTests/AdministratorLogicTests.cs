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
    private PersonRepositories _personRepo;
    private AdministratorLogic _administratorLogic;
    private AdministratorDto _administratorDto;
    private List<Booking> _bookings;
    private Promotion _promotion;
    private PromotionDto _promotionDto;
    private List<PromotionDto> _promotionsDto;
    private StorageUnit _storageUnit;
    private List<Promotion> _promotions;
    private BookingDto _bookingDto;
    private Booking _booking;
    private UserDto _userDto;
    private User _user;

    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _administratorLogic = new AdministratorLogic(_personRepo);
        _bookings = new List<Booking>();
        _promotions = new List<Promotion>();
        _promotionsDto = new List<PromotionDto>();
        _administratorDto = new AdministratorDto("Franco", "Ramos", "francoramos1511@gmail.com", "PassWord921#");
    }
    
    [TestMethod]
    public void WhenAdministratorApprovesABookingDtoShouldChangeItToTrue()
    {
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnit("12", AreaType.A, SizeType.Small, true, new List<Promotion>()), "");
        _bookings.Add(_booking);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookings);
        _personRepo.AddToRepository(_user);
        _promotionDto = new PromotionDto("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotionsDto.Add(_promotionDto);
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto), "");
        _userDto = new UserDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<BookingDto>());
        _administratorLogic.ApproveBooking(_userDto, _bookingDto);
    }

    [TestMethod]
    public void WhenAdministratorRejectsABookingDtoShouldWriteARejectionMessage()
    {
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnit("12", AreaType.A, SizeType.Small, true, new List<Promotion>()), "");
        _bookings.Add(_booking);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookings);
        _personRepo.AddToRepository(_user);
        _promotionDto = new PromotionDto("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotionsDto.Add(_promotionDto);
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto), "");
        _userDto = new UserDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<BookingDto>());
        string rejectionMessage = "The booking has been rejected";
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, rejectionMessage);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorRejectsABookingDtoWithEmptyMessageShouldThrowException()
    {
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnit("12", AreaType.A, SizeType.Small, true, new List<Promotion>()), "");
        _bookings.Add(_booking);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookings);
        _personRepo.AddToRepository(_user);
        _promotionDto = new PromotionDto("Winter Discount", 25, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15));
        _promotionsDto.Add(_promotionDto);
        _bookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto), "");
        _userDto = new UserDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<BookingDto>());
        _administratorLogic.SetRejectionMessage(_userDto, _bookingDto, "");
    }

    [TestMethod]
    public void WhenAdministratorIsTryingToApproveOrRejectBookingsShouldGetAListOfUsersDto()
    {
        _promotion = new Promotion("Winter Discount", 25, new DateTime(2023,7,5), new DateTime(2026,8,15));
        _promotions.Add(_promotion);
        _storageUnit = new StorageUnit("12", AreaType.A, SizeType.Small, true, _promotions);
        Booking booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), _storageUnit, "");
        _bookings.Add(booking);
        User user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookings);
        _personRepo.AddToRepository(user);
        List<UserDto> users = _administratorLogic.GetUsersDto();
        Assert.IsTrue(users.Count > 0);
    }
}