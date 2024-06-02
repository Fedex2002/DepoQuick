using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IPersonController
{
    public Person CreatePerson(PersonDto personDto);
    public void AddUser(PersonDto personDto);
    public void PayBooking(PersonDto personDto, BookingDto bookingDto);
    public List<PersonDto> GetPersonsDto();    
}