using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;
namespace Logic;

public class UserLogic
{
    private PersonRepositories _personRepo;
    
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
        Booking newBooking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        Person person = _personRepo.GetFromRepository(userDto.Email);
        bool exists = false;
        if (person is User user)
        {
            foreach (var oldBooking in user.GetBookings())
            {
                exists = IfStorageUnitInOldBookingAndNewBookingAreTheSameSetExistsToTrue(oldBooking, newBooking, exists);
            }
            if (!exists)
            {
                user.GetBookings().Add(newBooking);
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
        if (oldBooking.GetStorageUnit().GetId() == newBooking.GetStorageUnit().GetId() 
            && oldBooking.GetStorageUnit().GetArea() == newBooking.GetStorageUnit().GetArea() 
            && oldBooking.GetStorageUnit().GetSize() == newBooking.GetStorageUnit().GetSize() 
            && oldBooking.GetStorageUnit().GetClimatization() == newBooking.GetStorageUnit().GetClimatization() 
            && CheckIfPromotionsOfInStorageUnitOfOldBookingAndNewBookingAreTheSame(oldBooking, newBooking))
        {
            exists = true;
        }

        return exists;
    }
    
    private bool CheckIfPromotionsOfInStorageUnitOfOldBookingAndNewBookingAreTheSame(Booking oldBooking, Booking newBooking)
    {
        bool same = false;
        foreach (var promotion in oldBooking.GetStorageUnit().GetPromotions()) 
        { 
            same = CheckIfPromotionIsTheSame(promotion, newBooking.GetStorageUnit().GetPromotions());
        }
        return same;
    }
    
    private bool CheckIfPromotionIsTheSame(Promotion promotion, List<Promotion> promotions)
    {
        bool same = false;
        foreach (var promotion1 in promotions)
        {
            if (promotion.GetLabel() == promotion1.GetLabel() && promotion.GetDiscount() == promotion1.GetDiscount() && promotion.GetDateStart() == promotion1.GetDateStart() && promotion.GetDateEnd() == promotion1.GetDateEnd())
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
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        Person person = _personRepo.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            user.GetBookings().Remove(booking);
        }
    }
    
    public StorageUnit ChangeToStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = new List<Promotion>();
        foreach (var promotionDto in storageUnitDto.Promotions)
        {
            promotions.Add(new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd));
        }
        return new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions);
    }
    
    public double CalculateTotalPriceOfBooking(BookingDto bookingDto)
    {
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        return booking.CalculateBookingTotalPrice();
    }

    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto)
    {
        StorageUnit storageUnit = ChangeToStorageUnit(storageUnitDto);
        return storageUnit.CalculateStorageUnitPricePerDay();
    }
}