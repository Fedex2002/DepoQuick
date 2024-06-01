using Model;

namespace Repositories;

public class BookingRepositories
{
    private readonly List<Booking> _bookings = new List<Booking>();
    
    public void AddToRepository(Booking booking)
    {
        _bookings.Add(booking);
    }
    
}