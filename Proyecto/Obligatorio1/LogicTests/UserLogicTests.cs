using System.Runtime.InteropServices.ComTypes;
using Logic;
using Logic.DTOs;
using Repositories;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class UserLogicTests
{
    private User _person;
    private UserDto _userDto;
    private UserLogic _userLogic;
    private PersonRepositories _personRepo;
    private PromotionDto _promotionDto;
    private List<PromotionDto> _promotionsDto;
    private StorageUnitDto _storageUnitDto;
    private BookingDto _mybookingDto;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _person = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<Booking>());
        _personRepo = new PersonRepositories();
        _promotionsDto = new List<PromotionDto>();
        _userLogic = new UserLogic(_personRepo);
        _personRepo.AddToRepository(_person);
        _userDto = new UserDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<BookingDto>());
        _storageUnitDto = new StorageUnitDto("",AreaType.A, SizeType.Small, true,new List<PromotionDto>());
        _mybookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnitDto, "");
    }
    
    [TestMethod]
    public void WhenCreatingABookingDtoEmptyShouldReturnEmptyBooking()
    {
        BookingDto bookingDto = new BookingDto();
        Assert.IsNotNull(bookingDto);
    }
    
    [TestMethod]
    public void WhenUserMakesABookingShouldAddItToHisListOfBookings()
    {
        _userLogic.AddBookingToUser(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    public void WhenUserBookingIsApprovedShouldReturnTrue()
    {
        _mybookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), _storageUnitDto, "");
        _userLogic.AddBookingToUser(_userDto, _mybookingDto);
        Assert.IsTrue(_userLogic.CheckIfBookingIsApproved(_mybookingDto));
    }
    
    [TestMethod]
    public void WhenUserBookingIsRejectedShouldEliminateBookingFromUserListOfBookings()
    {
        _userLogic.AddBookingToUser(_userDto, _mybookingDto);
        _userLogic.RemoveBookingFromUser(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    public void WhenAUserBookingIsAddedOrRemovedShouldChangeStorageUnitDtoToAStorageUnit()
    {
        User user = new User("Franco", "Ramos", "francoramos1511@gmail.com", "PassWord921#2", new List<Booking>());
        _personRepo.AddToRepository(user);
        _promotionDto = new PromotionDto("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionsDto.Add(_promotionDto);
        BookingDto bookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), new StorageUnitDto("", AreaType.A, SizeType.Small, true, _promotionsDto), "");
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, _userLogic.ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        user.GetBookings().Add(booking);
        user.GetBookings().Remove(booking);
    }
      
    [TestMethod]
    public void WhenUserSelectsStartDayAndEndDayOfBookingShouldShowTotalPrice()
    {
        Assert.AreEqual(2835, _userLogic.CalculateTotalPriceOfBooking(_mybookingDto));
    }
}