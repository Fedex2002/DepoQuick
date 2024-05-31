using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;
namespace Logic;

public class BookingLogic
{
    private readonly PersonRepositories _personRepo;
    
    public BookingLogic(PersonRepositories personRepo)
    {
        _personRepo = personRepo;
    }
    
    public void AddBookingToUser(UserDto userDto, BookingDto bookingDto)
    {
        CheckIfPersonIsAUserAddBooking(userDto, bookingDto);
    }

    private void CheckIfPersonIsAUserAddBooking(UserDto userDto, BookingDto bookingDto)
    {
        Booking newBooking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment,userDto.Email);
        Person person = _personRepo.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            bool exists = user.Bookings.Any(booking => booking.StorageUnit.Id == newBooking.StorageUnit.Id);
            if (!exists)
            {
                user.Bookings.Add(newBooking);
            }
            else
            {
                IfUserAlreadyBookTheStorageUnitThrowException();
            }
        }
    }

    private static void IfUserAlreadyBookTheStorageUnitThrowException()
    {
        throw new LogicExceptions("Booking for this StorageUnit already exists");
    }

    public bool CheckIfBookingIsApproved(BookingDto bookingDto)
    {
        return bookingDto.Approved;
    }
    
    public void RemoveBookingFromUser(UserDto userDto, BookingDto bookingDto)
    {
        CheckIfPersonIsAUserRemoveBooking(userDto, bookingDto);
    }

    private void CheckIfPersonIsAUserRemoveBooking(UserDto userDto, BookingDto bookingDto)
    {
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd,
            ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status,
            bookingDto.Payment,userDto.Email);
        Person person = _personRepo.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            user.Bookings.Remove(booking);
        }
    }
    
    public StorageUnit ChangeToStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = new List<Promotion>();
        foreach (var promotionDto in storageUnitDto.Promotions)
        {
            promotions.Add(new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd));
        }
        
        List<DateRange> availableDates = new List<DateRange>();
        foreach (var dateRangeDto in storageUnitDto.AvailableDates)
        {
            availableDates.Add(new DateRange(dateRangeDto.StartDate, dateRangeDto.EndDate));
        }
        return new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions, availableDates);
    }
    
    public double CalculateTotalPriceOfBooking(BookingDto bookingDto)
    {
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment);
        return booking.CalculateBookingTotalPrice();
    }

    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto)
    {
        StorageUnit storageUnit = ChangeToStorageUnit(storageUnitDto);
        bool promotionIsInDateRange = storageUnit.Promotions.Any(promotion => dateRangeDto.StartDate >= promotion.DateStart && dateRangeDto.EndDate <= promotion.DateEnd);
        if (!promotionIsInDateRange)
        {
            storageUnit.Promotions = new List<Promotion>();
        }
        return storageUnit.CalculateStorageUnitPricePerDay();
    }
    
    public void PayBooking(UserDto userDto, BookingDto bookingDto)
    {
        Person person = _personRepo.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            FindUserBookingAndSetPaymentToTrue(bookingDto, user);
        }
    }

    private static void FindUserBookingAndSetPaymentToTrue(BookingDto bookingDto, User user)
    {
        foreach (var booking in user.Bookings)
        {
            if (booking.StorageUnit.Id == bookingDto.StorageUnitDto.Id)
            {
                IfBookingPaymentIsAlreadyTrueThrowException(booking);
                booking.Payment = true;
            }
        }
    }

    private static void IfBookingPaymentIsAlreadyTrueThrowException(Booking booking)
    {
        if (booking.Payment)
        {
            throw new LogicExceptions("Booking already paid");
        }
    }
    
    public void CheckIfDateStartAndDateEndAreIncludedInDateRange(DateTime dateStart, DateTime dateEnd, DateRangeDto dateRangeDto)
    {
        if (!(dateStart >= dateRangeDto.StartDate && dateEnd <= dateRangeDto.EndDate))
        {
            throw new LogicExceptions("Date range is not included in the available date range");
        }
    }
}