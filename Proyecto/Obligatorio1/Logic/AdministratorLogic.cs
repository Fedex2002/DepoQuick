using Repositories;
using Logic.DTOs;
using Model;
using Model.Exceptions;
namespace Logic;

public class AdministratorLogic
{
    private PersonRepositories _personRepositories;

    public AdministratorLogic(PersonRepositories personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public void ApproveBooking(UserDto userDto, BookingDto bookingDto)
    {
        Booking oldBooking = new Booking(false, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        Booking newBooking = new Booking(true, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        Person person = _personRepositories.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            List<Booking> bookingsToRemove = new List<Booking>();
            foreach (var booking in user.GetBookings())
            { 
                CheckIfOldBookingAndBookingAreTheSameAddToListToRemove(booking, oldBooking, bookingsToRemove);
            }
            foreach (var bookingToRemove in bookingsToRemove)
            {
                user.GetBookings().Remove(bookingToRemove);
            }
            user.GetBookings().Add(newBooking);
        }
    }

    private static void CheckIfOldBookingAndBookingAreTheSameAddToListToRemove(Booking booking, Booking oldBooking,
        List<Booking> bookingsToRemove)
    {
        if (booking.GetApproved() == oldBooking.GetApproved() && 
            booking.GetDateStart() == oldBooking.GetDateStart() &&
            booking.GetDateEnd() == oldBooking.GetDateEnd() && 
            booking.GetStorageUnit().GetId() == oldBooking.GetStorageUnit().GetId() &&
            booking.GetRejectedMessage() == oldBooking.GetRejectedMessage())
        {
            bookingsToRemove.Add(booking);
        }
    }

    public void SetRejectionMessage(UserDto userDto, BookingDto bookingDto, string rejectionMessage)
    {
        if (rejectionMessage == "")
        {
            throw new LogicExceptions("The rejection message can't be empty.");
        }
        Booking oldBooking = new Booking(false, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage);
        Booking newBooking = new Booking(false, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), rejectionMessage);
        Person person = _personRepositories.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            List<Booking> bookingsToRemove = new List<Booking>();
            foreach (var booking in user.GetBookings())
            {
                CheckIfOldBookingAndBookingAreTheSameAddToListToRemove(booking, oldBooking, bookingsToRemove);
            }
            foreach (var bookingToRemove in bookingsToRemove)
            {
                user.GetBookings().Remove(bookingToRemove);
            }
            user.GetBookings().Add(newBooking);
        }
    }

    public List<UserDto> GetUsersDto()
    {
        List<Person> users = _personRepositories.GetAllFromRepository();
        List<UserDto> usersDto = new List<UserDto>();
        foreach (var person in users)
        {
            if (person is User user)
            {
                UserDto userDto = new UserDto(user.GetName(), user.GetSurname(), user.GetEmail(), user.GetPassword(), 
                    GetUserBookingsDto(user.GetBookings()));
                usersDto.Add(userDto);
            }
        }

        return usersDto;
    }

    private List<BookingDto> GetUserBookingsDto(List<Booking> bookings)
    {
        List<BookingDto> bookingsDto = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            BookingDto bookingDto = new BookingDto(booking.GetApproved(), booking.GetDateStart(), booking.GetDateEnd(),
                GetUserStorageUnitDto(booking.GetStorageUnit()), booking.GetRejectedMessage());
            bookingsDto.Add(bookingDto);
        }
        return bookingsDto;
    }
    
    private StorageUnitDto GetUserStorageUnitDto(StorageUnit storageUnit)
    {
        return new StorageUnitDto(storageUnit.GetId(), storageUnit.GetArea(), storageUnit.GetSize(),
            storageUnit.GetClimatization(), GetUserPromotionsDto(storageUnit.GetPromotions()));
    }
    
    private List<PromotionDto> GetUserPromotionsDto(List<Promotion> promotions)
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach (var promotion in promotions)
        {
            PromotionDto promotionDto = new PromotionDto(promotion.GetLabel(), promotion.GetDiscount(),
                promotion.GetDateStart(), promotion.GetDateEnd());
            promotionsDto.Add(promotionDto);
        }
        return promotionsDto;
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