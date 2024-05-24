using Repositories;
using Logic.DTOs;
using Model;
using Model.Exceptions;
namespace Logic;

public class AdministratorLogic
{
    private readonly PersonRepositories _personRepositories;

    public AdministratorLogic(PersonRepositories personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public void ApproveBooking(UserDto userDto, BookingDto bookingDto)
    {
        Booking oldBooking = new Booking(false, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment);
        Booking newBooking = new Booking(true, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment);
        Person person = _personRepositories.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            List<Booking> bookingsToRemove = new List<Booking>();
            foreach (var booking in user.Bookings)
            { 
                CheckIfOldBookingAndBookingAreTheSameAddToListToRemove(booking, oldBooking, bookingsToRemove);
            }
            foreach (var bookingToRemove in bookingsToRemove)
            {
                user.Bookings.Remove(bookingToRemove);
            }
            user.Bookings.Add(newBooking);
        }
    }

    private static void CheckIfOldBookingAndBookingAreTheSameAddToListToRemove(Booking booking, Booking oldBooking,
        List<Booking> bookingsToRemove)
    {
        if (booking.Approved == oldBooking.Approved && 
            booking.DateStart == oldBooking.DateStart &&
            booking.DateEnd == oldBooking.DateEnd && 
            booking.StorageUnit.Id == oldBooking.StorageUnit.Id &&
            booking.RejectedMessage == oldBooking.RejectedMessage)
        {
            bookingsToRemove.Add(booking);
        }
    }

    public void SetRejectionMessage(UserDto userDto, BookingDto bookingDto, string rejectionMessage)
    {
        IfRejectionMessageIsEmptyThrowException(rejectionMessage);
        Booking oldBooking = new Booking(false, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment);
        Booking newBooking = new Booking(false, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), rejectionMessage, bookingDto.Status, bookingDto.Payment);
        Person person = _personRepositories.GetFromRepository(userDto.Email);
        if (person is User user)
        {
            List<Booking> bookingsToRemove = new List<Booking>();
            foreach (var booking in user.Bookings)
            {
                CheckIfOldBookingAndBookingAreTheSameAddToListToRemove(booking, oldBooking, bookingsToRemove);
            }
            foreach (var bookingToRemove in bookingsToRemove)
            {
                user.Bookings.Remove(bookingToRemove);
            }
            user.Bookings.Add(newBooking);
        }
    }

    private static void IfRejectionMessageIsEmptyThrowException(string rejectionMessage)
    {
        if (rejectionMessage == "")
        {
            throw new LogicExceptions("The rejection message can't be empty.");
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
                UserDto userDto = new UserDto(user.Name, user.Surname, user.Email, user.Password, 
                    GetUserBookingsDto(user.Bookings));
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
            BookingDto bookingDto = new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd,
                GetUserStorageUnitDto(booking.StorageUnit), booking.RejectedMessage, booking.Status, booking.Payment);
            bookingsDto.Add(bookingDto);
        }
        return bookingsDto;
    }
    
    private StorageUnitDto GetUserStorageUnitDto(StorageUnit storageUnit)
    {
        return new StorageUnitDto(storageUnit.Id, storageUnit.Area, storageUnit.Size,
            storageUnit.Climatization, GetUserPromotionsDto(storageUnit.Promotions));
    }
    
    private List<PromotionDto> GetUserPromotionsDto(List<Promotion> promotions)
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach (var promotion in promotions)
        {
            PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount,
                promotion.DateStart, promotion.DateEnd);
            promotionsDto.Add(promotionDto);
        }
        return promotionsDto;
    }

    private StorageUnit ChangeToStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = new List<Promotion>();
        foreach (var promotionDto in storageUnitDto.Promotions)
        {
            promotions.Add(new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd));
        }
        return new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions);
    }
}