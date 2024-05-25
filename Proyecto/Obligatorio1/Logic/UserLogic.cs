using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;
namespace Logic;

public class UserLogic
{
    private readonly PersonRepositories _personRepo;
    
    public UserLogic(PersonRepositories personRepo)
    {
        _personRepo = personRepo;
    }
    
    public void AddBookingToUser(UserDto userDto, BookingDto bookingDto)
    {
        CheckIfPersonIsAUserAddBooking(userDto, bookingDto);
    }

    private void CheckIfPersonIsAUserAddBooking(UserDto userDto, BookingDto bookingDto)
    {
        Booking newBooking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment);
        Person person = _personRepo.GetFromRepository(userDto.Email);
        bool exists = false;
        if (person is User user)
        {
            foreach (var booking in user.Bookings)
            {
                exists = user.Bookings.Any(booking => booking.StorageUnit.Id == newBooking.StorageUnit.Id);
            }
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

    private bool IfStorageUnitInOldBookingAndNewBookingAreTheSameSetExistsToTrue(Booking oldBooking, Booking newBooking,
        bool exists)
    {
        if (oldBooking.StorageUnit.Id == newBooking.StorageUnit.Id 
            && oldBooking.StorageUnit.Area == newBooking.StorageUnit.Area
            && oldBooking.StorageUnit.Size == newBooking.StorageUnit.Size
            && oldBooking.StorageUnit.Climatization == newBooking.StorageUnit.Climatization
            && CheckIfPromotionsOfInStorageUnitOfOldBookingAndNewBookingAreTheSame(oldBooking, newBooking))
            
        {
            exists = true;
        }

        return exists;
    }
    
    private bool CheckIfPromotionsOfInStorageUnitOfOldBookingAndNewBookingAreTheSame(Booking oldBooking, Booking newBooking)
    {
        bool same = false;
        if ((oldBooking.StorageUnit.Promotions == null || oldBooking.StorageUnit.Promotions.Count == 0) &&
            (newBooking.StorageUnit.Promotions == null || newBooking.StorageUnit.Promotions.Count == 0))
        {
            same = true;
        }
        else
        {
            foreach (var promotion in oldBooking.StorageUnit.Promotions) 
            { 
                same = CheckIfPromotionIsTheSame(promotion, newBooking.StorageUnit.Promotions);
            }
        }
        return same;
    }
    
    private bool CheckIfPromotionIsTheSame(Promotion promotion, List<Promotion> promotions)
    {
        bool same = false;
        foreach (var promotion1 in promotions)
        {
            if (promotion.Label == promotion1.Label && promotion.Discount == promotion1.Discount && promotion.DateStart == promotion1.DateStart && promotion.DateEnd == promotion1.DateEnd)
            {
                same = true;
            }
        }
        return same;
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
            bookingDto.Payment);
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

    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto)
    {
        StorageUnit storageUnit = ChangeToStorageUnit(storageUnitDto);
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
}