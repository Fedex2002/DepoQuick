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
    
    public void AddBookingToUser(Person person, BookingDto bookingDto)
    {
        CheckIfPersonIsAUserAddBooking(person, bookingDto);
    }

    private void CheckIfPersonIsAUserAddBooking(Person person, BookingDto bookingDto)
    {
        if (person is User user)
        {
            Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedBooking);
            user.GetBookings().Add(booking);
        }
        else
        {
            PersonIsNotAUserSoThrowException();
        }
    }
    
    public bool CheckIfBookingIsApproved(BookingDto bookingDto)
    {
        return bookingDto.Approved;
    }
    
    public void RemoveBookingFromUser(Person person, BookingDto bookingDto)
    {
        CheckIfPersonIsAUserRemoveBooking(person, bookingDto);
    }

    private void CheckIfPersonIsAUserRemoveBooking(Person person, BookingDto bookingDto)
    {
        if (person is User user)
        {
            Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedBooking);
            user.GetBookings().Remove(booking);
        }
        else
        {
            PersonIsNotAUserSoThrowException();
        }
    }

    private static void PersonIsNotAUserSoThrowException()
    {
        throw new LogicExceptions("The person is not a user");
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