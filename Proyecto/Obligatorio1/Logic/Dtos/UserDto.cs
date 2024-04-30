using Model;

namespace Logic.DTOs;

public class UserDto : PersonDto
{
    public List<Booking> Bookings = new List<Booking>();
    public UserDto()
    {
    }
 
    public UserDto(string name, string surname, string email, string password,List<Booking> bookings) : base(name, surname, email, password)
    {
        Bookings = bookings;
    }
}