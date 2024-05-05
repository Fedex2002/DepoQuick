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
    private StorageUnit _storageUnit;
    private List<Promotion> _promotions;
    private BookingDto _bookingDto;

    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _administratorLogic = new AdministratorLogic(_personRepo);
        _bookings = new List<Booking>();
        _promotions = new List<Promotion>();
        _administratorDto = new AdministratorDto("Franco", "Ramos", "francoramos1511@gmail.com", "PassWord921#");
    }
    
    [TestMethod]
    public void WhenAdministratorApprovesABookingDtoShouldChangeItToTrue()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, new List<PromotionDto>()), "");
        _bookingDto = _administratorLogic.ApproveBooking(_bookingDto);
        Assert.AreEqual(true, _bookingDto.Approved);
    }

    [TestMethod]
    public void WhenAdministratorRejectsABookingDtoShouldWriteARejectionMessage()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, new List<PromotionDto>()), "");
        string rejectionMessage = "The booking is rejected";
        _bookingDto = _administratorLogic.SetRejectionMessage(_bookingDto, rejectionMessage);
        Assert.IsTrue(_bookingDto.RejectedMessage.Length > 0);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorRejectsABookingDtoWithEmptyMessageShouldThrowException()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, new List<PromotionDto>()), "");
        string rejectionMessage = "";
        _bookingDto = _administratorLogic.SetRejectionMessage(_bookingDto, rejectionMessage);
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