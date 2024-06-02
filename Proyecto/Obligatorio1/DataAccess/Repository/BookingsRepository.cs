using DataAccess.Context;
using Model;

namespace DataAccess.Repository;

public class BookingsRepository
{
    private ApplicationDbContext _database;
    
    public BookingsRepository(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void AddBooking(Booking booking)
    {
        _database.Bookings.Add(booking);

        _database.SaveChanges();
    }
}