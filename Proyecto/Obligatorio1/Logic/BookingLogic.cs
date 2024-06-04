using DataAccess.Repository;
using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;
namespace Logic;

public class BookingLogic
{
    private readonly BookingsRepository _bookingRepositories;
    
    public BookingLogic(BookingsRepository bookingRepo)
    {
        _bookingRepositories = bookingRepo;
    }
    
    public void AddBooking(PersonDto userDto, BookingDto bookingDto)
    {
        CheckIfAlreadyBookedAndAddBooking(userDto, bookingDto);
    }

    private void CheckIfAlreadyBookedAndAddBooking(PersonDto userDto, BookingDto bookingDto)
    {
      
            List<Booking> bookings = _bookingRepositories.GetAllBookings();
            bool exists = bookings.Any(booking => booking.StorageUnit.Id == bookingDto.StorageUnitDto.Id && booking.PersonEmail == userDto.Email);
            if (!exists)
            {
                Booking newBooking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment,userDto.Email);
                _bookingRepositories.AddBooking(newBooking);
            }
            else
            {
                IfUserAlreadyBookTheStorageUnitThrowException();
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
    
    public void RemoveBookingFromUser( BookingDto bookingDto)
    {
        RemoveBookingFromPerson( bookingDto);
    }

    private void RemoveBookingFromPerson(BookingDto bookingDto)
    {
        Booking booking = _bookingRepositories.FindBookingByStorageUnitIdAndEmail(bookingDto.StorageUnitDto.Id, bookingDto.UserEmail);
       _bookingRepositories.DeleteBooking(booking);
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
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment, bookingDto.UserEmail);
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
    
    public void PayBooking(PersonDto userDto, BookingDto bookingDto)
    {
        List<Booking> bookings = _bookingRepositories.GetAllBookings();
        var bookingToPay = bookings.FirstOrDefault(
            b => b.PersonEmail == userDto.Email && b.StorageUnit.Id == bookingDto.StorageUnitDto.Id
        );

        if (bookingToPay != null)
        {
            IfBookingPaymentIsAlreadyTrueThrowException(bookingToPay);
            bookingToPay.Payment = true;
            bookingDto.Payment = true;
        }
    }
    
    public List<BookingDto> GetAllBookingsDto()
    {
        List<BookingDto> bookingsDto = new List<BookingDto>();
        foreach (var booking in _bookingRepositories.GetAllBookings())
        {
            bookingsDto.Add(new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd, new StorageUnitDto(booking.StorageUnit.Id, booking.StorageUnit.Area, booking.StorageUnit.Size, booking.StorageUnit.Climatization, new List<PromotionDto>(), new List<DateRangeDto>()), booking.RejectedMessage, booking.Status, booking.Payment, booking.PersonEmail));
        }
        return bookingsDto;
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