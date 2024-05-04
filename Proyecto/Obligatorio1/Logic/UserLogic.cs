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
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        Person person = _personRepo.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            user.GetBookings().Add(booking);
        }
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
}