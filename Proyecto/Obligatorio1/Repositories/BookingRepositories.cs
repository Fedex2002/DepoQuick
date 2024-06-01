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
    
    public bool ExistsInRepository(string email)
    {
        return _bookings.Any(u => u.PersonEmail == email);
    }
    
    public void RemoveFromRepository(Booking booking)
    {
        _bookings.Remove(booking);
    }
    
    public List<Booking> GetAllFromRepository()
    {
        return _bookings;
    }
    
}