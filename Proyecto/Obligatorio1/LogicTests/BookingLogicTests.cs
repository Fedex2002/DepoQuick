using System.Runtime.InteropServices.ComTypes;
using Logic;
using Logic.DTOs;
using Repositories;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class BookingLogicTests
{
    private Person _person;
    private PersonDto _userDto;
    private BookingLogic _bookingLogic;
    private BookingRepositories _bookingRepo;
    private PromotionDto _promotionDto;
    private List<PromotionDto> _promotionsDto;
    private StorageUnitDto _storageUnitDto;
    private BookingDto _mybookingDto;
    private List<DateRangeDto> _availableDatesDto;
    private DateRangeDto _dateRangeDto;
    private Booking _booking;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
        _bookingRepo = new BookingRepositories();
        _promotionsDto = new List<PromotionDto>();
        _booking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnitDto, "", "Reservado", false,_person.Email);
        _availableDatesDto = new List<DateRangeDto>();
        _promotionDto = new PromotionDto("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionsDto.Add(_promotionDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 1), new DateTime(2024, 8, 15));
        _availableDatesDto.Add(_dateRangeDto);
        _bookingLogic = new BookingLogic(_bookingRepo);
        _bookingRepo.AddToRepository(_booking);
        _userDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
        _storageUnitDto = new StorageUnitDto("",AreaType.A, SizeType.Small, true,_promotionsDto, _availableDatesDto);
        _mybookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnitDto, "", "Reservado", false,_person.Email);
    }
    
    [TestMethod]
    public void WhenCreatingABookingDtoEmptyShouldReturnEmptyBooking()
    {
        BookingDto bookingDto = new BookingDto();
        Assert.IsNotNull(bookingDto);
    }
    
    [TestMethod]
    public void WhenUserMakesABookingShouldAddItToBookingRepositories()
    {
        _bookingLogic.AddBooking(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    public void WhenUserBookingIsApprovedShouldReturnTrue()
    {
        _mybookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), _storageUnitDto, "", "Reservado", false,_userDto.Email);
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
        Assert.IsTrue(_bookingLogic.CheckIfBookingIsApproved(_mybookingDto));
    }
    
    [TestMethod]
    public void WhenUserBookingIsRejectedShouldEliminateBookingFromUserListOfBookings()
    {
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
        _bookingLogic.RemoveBookingFromUser(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    public void WhenAUserBookingIsAddedOrRemovedShouldChangeStorageUnitDtoToAStorageUnit()
    {
        User user = new User("Franco", "Ramos", "francoramos1511@gmail.com", "PassWord921#2", new List<Booking>());
        _bookingRepo.AddToRepository(user);
        BookingDto bookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), new StorageUnitDto("", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "", "Reservado", false,_userDto.Email);
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, _bookingLogic.ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment,bookingDto.UserEmail);
        user.Bookings.Add(booking);
        user.Bookings.Remove(booking);
    }
      
    [TestMethod]
    public void WhenUserSelectsStartDayAndEndDayOfBookingShouldShowTotalPrice()
    {
        Assert.AreEqual(2126.25, _bookingLogic.CalculateTotalPriceOfBooking(_mybookingDto));
    }
    
    [TestMethod]
    public void WhenUserEntersPageBookingsShouldShowPricePerDayOfStorageUnit()
    {
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        Assert.AreEqual(52.5, _bookingLogic.CalculateStorageUnitPricePerDay(_storageUnitDto, _dateRangeDto));
        
        Assert.AreEqual(70, _bookingLogic.CalculateStorageUnitPricePerDay(_storageUnitDto, _storageUnitDto.AvailableDates[0]));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenUserTriesToBookTheSameStorageUnitWithPromotionTwiceShouldThrowException()
    {
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenUserTriesToBookTheSameStorageUnitWithoutPromotionTwiceShouldThrowException()
    {
        _storageUnitDto = new StorageUnitDto("",AreaType.A, SizeType.Small, true, new List<PromotionDto>(), new List<DateRangeDto>());
        _mybookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnitDto, "", "Reservado", false, _userDto.Email);
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
    }

    [TestMethod]
    public void WhenUserPaysABookingShouldSetItToTrue()
    {
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
        _bookingLogic.PayBooking(_userDto, _mybookingDto);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenUserTriesToPayABookingTwiceShouldThrowException()
    {
        _bookingLogic.AddBookingToUser(_userDto, _mybookingDto);
        _bookingLogic.PayBooking(_userDto, _mybookingDto);
        _bookingLogic.PayBooking(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void IfSelectedStartDateAndEndDateOfBookingIsNotInDateRangeShouldThrowException()
    {
        DateTime startDate = new DateTime(2024, 10, 15);
        DateTime endDate = new DateTime(2024, 10, 30);
        _bookingLogic.CheckIfDateStartAndDateEndAreIncludedInDateRange(startDate, endDate, _dateRangeDto);
    }
}