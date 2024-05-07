namespace Model;

public class User : Person
{
    private readonly List<Booking> _bookings;
    public User()
    {
    }
    
    public User(string name, string surname, string email, string password, List<Booking> bookings) : base(name, surname, email, password)
    {
        _bookings = bookings;
    }
    
    public List<Booking> GetBookings()
    {
        return _bookings;
    }
}