using Repositories;
using Logic.DTOs;
using Model.Exceptions;
namespace Logic;

public class AdministratorLogic
{
    private PersonRepositories _personRepositories;
    
    public AdministratorLogic(PersonRepositories personRepositories)
    {
        _personRepositories = personRepositories;
    }
    
    public BookingDto ApproveBooking(BookingDto bookingDto)
    {
        return new BookingDto(true, bookingDto.DateStart, bookingDto.DateEnd, bookingDto.StorageUnitDto, bookingDto.RejectedMessage);
    }
    
    public BookingDto SetRejectionMessage(BookingDto bookingDto, string rejectionMessage)
    {
        if (rejectionMessage.Length == 0)
        {
            throw new LogicExceptions("The rejection message cannot be empty");
        }
        else
        {
            return new BookingDto(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, bookingDto.StorageUnitDto, rejectionMessage);
        }
    }
    
    public List<UserDto> GetUsersDto()
    {
        List<UserDto> users = new List<UserDto>();
        UserDto userDto = new UserDto("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<BookingDto>());
        users.Add(userDto);
        return users;
    }
}