using Model;

namespace Repositories;

public class BookingRepositories
{
    private readonly List<Booking> _bookings = new List<Booking>();
    
    public void AddToRepository(Booking booking)
    {
        _bookings.Add(booking);
    }
    
    public Booking GetFromRepository(string email)
    {
        Booking bookingInRepo = _bookings.Find(u => u.PersonEmail == email);
        return bookingInRepo;
    }
    
}