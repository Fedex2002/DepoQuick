using System.Runtime.InteropServices.ComTypes;
using Logic;
using Logic.DTOs;
using Repositories;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class BookingControllerTests
{
    private Person _person;
    private PersonDto _userDto;
    private BookingController _bookingController;
    private BookingRepositories _bookingRepo;
    private PromotionDto _promotionDto;
    private List<PromotionDto> _promotionsDto;
    private StorageUnit _storageUnit;
    private StorageUnitDto _storageUnitDto;
    private BookingDto _mybookingDto;
    private List<DateRangeDto> _availableDatesDto;
    private DateRangeDto _dateRangeDto;
    private Booking _booking;
    private List<Promotion> _promotions;
    private Promotion _promotion;
    private List<DateRange> _availableDates;
    private DateRange _dateRange;
    private PersonDto _personDto;

    [TestInitialize]
    public void TestInitialize()
    {
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
        _promotions = new List<Promotion>();
        _bookingRepo = new BookingRepositories();
        _promotionsDto = new List<PromotionDto>();
        _promotion = new Promotion("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotions.Add(_promotion);
        _availableDates = new List<DateRange>();
        _dateRange = new DateRange(new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _availableDates.Add(_dateRange);
        _storageUnit= new StorageUnit("1", AreaType.B, SizeType.Medium, false, _promotions, _availableDates);
        _availableDatesDto = new List<DateRangeDto>();
        _promotionDto = new PromotionDto("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionsDto.Add(_promotionDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 1), new DateTime(2024, 8, 15));
        _availableDatesDto.Add(_dateRangeDto);
        _bookingController = new BookingController(_bookingRepo);
        _userDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
        _storageUnitDto = new StorageUnitDto("", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto);
        _mybookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnitDto, "", "Reservado", false, _person.Email);
        _personDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
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
        _bookingController.AddBooking(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    public void WhenUserBookingIsApprovedShouldReturnTrue()
    {
        _mybookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), _storageUnitDto, "", "Reservado", false,_userDto.Email);
        _bookingController.AddBooking(_userDto, _mybookingDto);
        Assert.IsTrue(_bookingController.CheckIfBookingIsApproved(_mybookingDto));
    }
    
    [TestMethod]
    public void WhenUserBookingIsRejectedShouldEliminateBookingFromUserListOfBookings()
    {
        _bookingController.AddBooking(_userDto, _mybookingDto);
        _bookingController.RemoveBookingFromUser(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    public void WhenAUserBookingIsAddedOrRemovedShouldChangeStorageUnitDtoToAStorageUnit()
    {

        BookingDto bookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), new StorageUnitDto("", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "", "Reservado", false,_userDto.Email);
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, _bookingController.ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment,bookingDto.UserEmail);
        _bookingRepo.AddToRepository(_booking);
        _bookingRepo.RemoveFromRepository(_booking);
    }
      
    [TestMethod]
    public void WhenUserSelectsStartDayAndEndDayOfBookingShouldShowTotalPrice()
    {
        Assert.AreEqual(2126.25, _bookingController.CalculateTotalPriceOfBooking(_mybookingDto));
    }
    
    [TestMethod]
    public void WhenUserEntersPageBookingsShouldShowPricePerDayOfStorageUnit()
    {
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        Assert.AreEqual(52.5, _bookingController.CalculateStorageUnitPricePerDay(_storageUnitDto, _dateRangeDto));
        
        Assert.AreEqual(70, _bookingController.CalculateStorageUnitPricePerDay(_storageUnitDto, _storageUnitDto.AvailableDates[0]));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenUserTriesToBookTheSameStorageUnitWithPromotionTwiceShouldThrowException()
    {
        _bookingController.AddBooking(_userDto, _mybookingDto);
        _bookingController.AddBooking(_userDto, _mybookingDto);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenUserTriesToBookTheSameStorageUnitWithoutPromotionTwiceShouldThrowException()
    {
        _storageUnitDto = new StorageUnitDto("",AreaType.A, SizeType.Small, true, new List<PromotionDto>(), new List<DateRangeDto>());
        _mybookingDto = new BookingDto(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnitDto, "", "Reservado", false, _userDto.Email);
        _bookingController.AddBooking(_userDto, _mybookingDto);
        _bookingController.AddBooking(_userDto, _mybookingDto);
    }

    [TestMethod]
    public void WhenUserPaysABookingShouldSetItToTrue()
    {
        _bookingController.AddBooking(_userDto, _mybookingDto);
        _bookingController.PayBooking(_userDto, _mybookingDto);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenUserTriesToPayABookingTwiceShouldThrowException()
    {
        _bookingController.AddBooking(_userDto, _mybookingDto);
        _bookingController.PayBooking(_userDto, _mybookingDto);
        _bookingController.PayBooking(_userDto, _mybookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void IfSelectedStartDateAndEndDateOfBookingIsNotInDateRangeShouldThrowException()
    {
        DateTime startDate = new DateTime(2024, 10, 15);
        DateTime endDate = new DateTime(2024, 10, 30);
        _bookingController.CheckIfDateStartAndDateEndAreIncludedInDateRange(startDate, endDate, _dateRangeDto);
    }

    [TestMethod]
    public void WhenGettingAllBookingsDtoShouldReturnThem()
    {
        _bookingController.AddBooking(_userDto, _mybookingDto);
        _storageUnitDto = new StorageUnitDto("hola",AreaType.A, SizeType.Small, true,_promotionsDto, _availableDatesDto);
        BookingDto booking2 = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            _storageUnitDto, "", "Reservado", false, "samplemail@gmail.com");
        _bookingController.AddBooking(_userDto, booking2);
        List<BookingDto> bookings = _bookingController.GetAllBookingsDto();
        Assert.AreEqual(2, bookings.Count);
    }
    
    [TestMethod]
    public void WhenAdministratorRejectsABookingDtoShouldWriteARejectionMessage()
    {
        _mybookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", true,_person.Email);
        string rejectionMessage = "The booking has been rejected";
        _bookingController.AddBooking(_personDto, _mybookingDto);
        _bookingController.SetRejectionMessage(_personDto, _mybookingDto, rejectionMessage);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorRejectsABookingDtoWithEmptyMessageShouldThrowException()
    {
        _bookingController.SetRejectionMessage(_personDto, _mybookingDto, "");
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyApprovedABookingShouldThrowException()
    {
        _mybookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", true,_person.Email);
        _bookingController.ApproveBooking(_personDto, _mybookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyRejectedABookingShouldThrowException()
    {
        _mybookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "Rejected",
            "Reservado", true,_person.Email);
        _bookingController.SetRejectionMessage(_personDto, _mybookingDto, "Rejected");
    }
    
    [TestMethod] 
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyRejectedABookingAndTriesToApproveItShouldThrowException()
    {
        _mybookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "Rejected",
            "Reservado", false,_person.Email);
        _bookingController.ApproveBooking(_personDto, _mybookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorAlreadyApprovedABookingAndTriesToRejectItShouldThrowException()
    {
        _mybookingDto = new BookingDto(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnitDto("12", AreaType.A, SizeType.Small, true, _promotionsDto, _availableDatesDto), "",
            "Reservado", false,_person.Email);
        _bookingController.SetRejectionMessage(_personDto, _mybookingDto, "Rejected");
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorTriesToApproveABookingAndUserDidNotMakeThePaymentShouldThrowException()
    {
        _bookingController.ApproveBooking(_personDto, _mybookingDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenAdministratorTriesToRejectABookingAndUserDidNotMakeThePaymentShouldThrowException()
    {
        _bookingController.SetRejectionMessage(_personDto, _mybookingDto, "Rejected");
    }
}