using Model;

namespace Logic.DTOs;

public class UserDto : PersonDto
{
    public List<BookingDto> Bookings { get; set; }
    public UserDto()
    {
        Bookings = new List<BookingDto>();
    }
 
    public UserDto(string name, string surname, string email, string password,List<BookingDto> bookings) : base(name, surname, email, password)
    {
        Bookings = bookings;
    }
}