using Model;
using Model.Enums;
using Repositories;

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
        Booking bookingInRepo = _bookingRepositories.GetFromRepository(_booking.PersonEmail);
        Assert.AreEqual(_booking.PersonEmail, bookingInRepo.PersonEmail);
    }
    
    [TestMethod]
    public void WhenGettingBookingFromRepositoryShouldReturnIt()
    {
        _bookingRepositories.AddToRepository(_booking);
        Booking bookingInRepo = _bookingRepositories.GetFromRepository(_booking.PersonEmail);
        Assert.AreEqual(_booking.PersonEmail, bookingInRepo.PersonEmail);
    }
    
    [TestMethod]
    public void WhenRemovingBookingFromRepositoryShouldRemoveIt()
    {
        _bookingRepositories.AddToRepository(_booking);
        _bookingRepositories.RemoveFromRepository(_booking);
        Assert.IsFalse(_bookingRepositories.ExistsInRepository(_booking.PersonEmail));
    }
    
 
}