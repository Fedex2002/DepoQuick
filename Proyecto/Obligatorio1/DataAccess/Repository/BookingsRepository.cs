using DataAccess.Context;
using Model;
using Model.Exceptions;

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
        if (BookingAlreadyExists(booking))
        {
            BookingAlreadyExistsSoThrowException();
        }
        
        _database.Bookings.Add(booking);

        _database.SaveChanges();
    }

    private static void BookingAlreadyExistsSoThrowException()
    {
        throw new RepositoryExceptions("Booking already exists");
    }

    public bool BookingAlreadyExists(Booking booking)
    {
        return _database.Bookings.Any(b => b == booking);
    }
}