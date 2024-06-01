using Model;
using Model.Enums;

namespace RepositoriesTests;
[TestClass]
public class BookingRepositoriesTests
{
    private BookingRepositories _bookingRepositories;
    private Booking _booking;

    [TestInitialize]
    public void TestInitialize()
    {
        _bookingRepositories = new BookingRepositories();
        _booking = new Booking(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15),
            new StorageUnit("12", AreaType.A, SizeType.Small, true, new List<Promotion>(), new List<DateRange>()), "",
            "Reservado", false, "samplemail@gmail.com");
    }
    
    [TestMethod]
    public void WhenAddingNewBookingShouldAddItToRepository()
    {
        _bookingRepositories.AddToRepository(_booking);
        Booking bookingInRepo = _bookingRepositories.GetFromRepository(_booking.Id);
        Assert.AreEqual(_booking.PersonEmail, bookingInRepo.PersonEmail);
    }
}