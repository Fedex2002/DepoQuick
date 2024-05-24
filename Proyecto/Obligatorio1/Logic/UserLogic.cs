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
                exists = IfStorageUnitInOldBookingAndBookingAreTheSameSetExistsToTrue(booking, newBooking, exists);
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

    private static bool IfStorageUnitInOldBookingAndBookingAreTheSameSetExistsToTrue(Booking booking, Booking newBooking,
        bool exists)
    {
        if (booking.StorageUnit.Id == newBooking.StorageUnit.Id 
            && booking.StorageUnit.Area == newBooking.StorageUnit.Area 
            && booking.StorageUnit.Size == newBooking.StorageUnit.Size 
            && booking.StorageUnit.Climatization == newBooking.StorageUnit.Climatization 
            && booking.StorageUnit.Promotions.SequenceEqual(newBooking.StorageUnit.Promotions))
        {
            exists = true;
        }

        return exists;
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
        return new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions);
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
}