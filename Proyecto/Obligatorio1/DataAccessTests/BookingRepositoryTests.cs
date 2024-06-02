using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Enums;

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
}