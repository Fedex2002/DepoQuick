using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
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

    public bool BookingAlreadyExists(Booking booking)
    {
        return _database.Bookings.Any(b => b.PersonEmail == booking.PersonEmail && b.StorageUnit.Id == booking.StorageUnit.Id);
    }
    
    public List<Booking> GetAllBookings()
    {
        return _database.Bookings
            .Include(b => b.StorageUnit)         
            .ThenInclude(s => s.Promotions)       
            .Include(b => b.StorageUnit)         
            .ThenInclude(s => s.AvailableDates)    
            .ToList();
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
    
    public void UpdateBooking(Booking booking)
    {
        _database.Bookings.Update(booking);
        _database.SaveChanges();
    }
}