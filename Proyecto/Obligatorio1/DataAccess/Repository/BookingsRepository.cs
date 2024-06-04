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
        if (BookingExists(booking))
        {
            BookingAlreadyExistsSoThrowException();
        }

        AddNewBookingToBookingsTable(booking);
    }

    private void AddNewBookingToBookingsTable(Booking booking)
    {
        _database.Bookings.Add(booking);

        _database.SaveChanges();
    }

    private static void BookingAlreadyExistsSoThrowException()
    {
        throw new RepositoryExceptions("Booking already exists");
    }

    public bool BookingExists(Booking booking)
    {
        return _database.Bookings.Any(b => b == booking);
    }
    
    public List<Booking> GetAllBookings()
    {
        return _database.Bookings.ToList();
    }
    
    public Booking FindBookingByStorageUnitIdAndEmail(string storageUnitId, string email)
    {
        return _database.Bookings.FirstOrDefault(b => b.StorageUnit.Id == storageUnitId && b.PersonEmail == email);
    }

    public void DeleteBooking(Booking booking)
    {
        _database.Bookings.Remove(booking);
        _database.SaveChanges();
    }
}