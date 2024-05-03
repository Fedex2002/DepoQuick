using Repositories;
using Logic.DTOs;
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
        return new BookingDto(true, bookingDto.DateStart, bookingDto.DateEnd, bookingDto.StorageUnitDto, bookingDto.RejectedBooking);
    }
    
}