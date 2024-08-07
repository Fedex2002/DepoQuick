using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace DataAccessTests;

[TestClass]
public class BookingRepositoryTests
{
    private BookingsRepository _repository;
    private ApplicationDbContext _context;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private Booking _booking;

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _repository = new BookingsRepository(_context);
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnit("12", AreaType.A, SizeType.Small, true, new List<Promotion>(), new List<DateRange>()), "",
            "Reservado", false, "samplemail@gmail.com");
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void WhenAddingANewBooking_ShouldAddTheNewBookingInBookingsTable()
    {
        _repository.AddBooking(_booking);

        var bookingInDb = _context.Bookings.First();

        Assert.AreEqual(_booking, bookingInDb);
    }

    [TestMethod]
    public void WhenBookingExists_ShouldReturnTrue()
    {
        _repository.AddBooking(_booking);

        bool exists = _repository.BookingAlreadyExists(_booking);

        Assert.IsTrue(exists);
    }

    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenBookingAlreadyExists_ShouldThrowRepositoryException()
    {
        _repository.AddBooking(_booking);
        _repository.AddBooking(_booking);
    }

    [TestMethod]
    public void WhenGettingAllBookings_ShouldReturnAllTheBookingsInTheDatabase()
    {
        _repository.AddBooking(_booking);

        List<Booking> bookings = _repository.GetAllBookings();

        Assert.AreEqual(1, bookings.Count);
    }

    [TestMethod]
    public void WhenTryingToFindABooking_ShouldReturnBookingIfItIsInTheDatabase()
    {
        _repository.AddBooking(_booking);

        Booking bookingInDb = _repository.FindBookingByStorageUnitIdAndEmail(_booking.StorageUnit.Id, _booking.PersonEmail);

        Assert.AreEqual(_booking, bookingInDb);
    }
    
    [TestMethod]
    
    public void WhenDeletingABooking_ShouldRemoveTheBookingFromTheDatabase()
    {
        _repository.AddBooking(_booking);
        _repository.DeleteBooking(_booking);
        List<Booking> bookings = _repository.GetAllBookings();
        Assert.AreEqual(0, bookings.Count);
    }

    [TestMethod]
    public void WhenBookingIsApproved_ShouldChangeItInTheDatabase()
    {
        _repository.AddBooking(_booking);
        _booking.Approved = true;
        _booking.Status = "Capturado";
        _repository.UpdateBooking(_booking);
        Booking bookingInDb = _repository.FindBookingByStorageUnitIdAndEmail(_booking.StorageUnit.Id, _booking.PersonEmail);
        Assert.IsTrue(bookingInDb.Approved);
        Assert.AreEqual("Capturado", bookingInDb.Status);
    }
    
    [TestMethod]
    public void WhenBookingIsRejected_ShouldChangeItInTheDatabase()
    {
        _repository.AddBooking(_booking);
        _booking.RejectedMessage = "Rejected";
        _booking.Status = "Rechazado";
        _repository.UpdateBooking(_booking);
        Booking bookingInDb = _repository.FindBookingByStorageUnitIdAndEmail(_booking.StorageUnit.Id, _booking.PersonEmail);
        Assert.AreEqual("Rejected", bookingInDb.RejectedMessage);
        Assert.AreEqual("Rechazado", bookingInDb.Status);
    }
    
    [TestMethod]
    public void WhenUserPaysForBooking_ShouldChangeItInTheDatabase()
    {
        _repository.AddBooking(_booking);
        _booking.Payment = true;
        _repository.UpdateBooking(_booking);
        Booking bookingInDb = _repository.FindBookingByStorageUnitIdAndEmail(_booking.StorageUnit.Id, _booking.PersonEmail);
        Assert.IsTrue(bookingInDb.Payment);
    }

    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenDeletingBookingThatDoesNotExist_ShouldThrowRepositoryException()
    {
        _repository.DeleteBooking(_booking);
    }

}