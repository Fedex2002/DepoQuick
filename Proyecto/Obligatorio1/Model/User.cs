namespace Model;

public class User : Person
{
    private Booking _booking;
    public User()
    {
    }
    
    public User(string name, string surname, string email, string password, Booking booking) : base(name, surname, email, password)
    {
        _booking = booking;
    }
    
    public Booking GetBooking()
    {
        return _booking;
    }
}