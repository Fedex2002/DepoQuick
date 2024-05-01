using System.Runtime.InteropServices.ComTypes;
using Logic;
using Repositories;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class UserLogicTests
{
    private User _person;
    private UserLogic _userLogic;
    private PersonRepositories _personRepo;
    private Promotion _promotion;
    private StorageUnit _storageUnit;
    private Booking _mybooking;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _person = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<Booking>());
        _personRepo = new PersonRepositories();
        _userLogic = new UserLogic(_personRepo);
        _personRepo.AddToRepository(_person);
        _storageUnit = new StorageUnit("",AreaType.A, SizeType.Small, true,new List<Promotion>());
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnit, "");
    }
    
    [TestMethod]
    public void WhenUserMakesABookingShouldAddItToHisListOfBookings()
    {
        _userLogic.AddBookingToUser(_personRepo.GetFromRepository(_person.GetEmail()), _mybooking);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenSomeoneThatIsNotAUserMakesABookingShouldThrowException()
    {
        Administrator admin = new Administrator("Franco", "Ramos", "francoramos@gmail.com", "PassWord921#2");
        _userLogic.AddBookingToUser(_personRepo.GetFromRepository(admin.GetEmail()), _mybooking);
    }
    
    [TestMethod]
    public void WhenUserBookingIsApprovedShouldReturnTrue()
    {
        _mybooking = new Booking(true, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), _storageUnit, "");
        _userLogic.AddBookingToUser(_personRepo.GetFromRepository(_person.GetEmail()), _mybooking);
        Assert.IsTrue(_userLogic.CheckIfBookingIsApproved(_mybooking));
    }
    
    [TestMethod]
    public void WhenUserBookingIsRejectedShouldEliminateBookingFromUserListOfBookings()
    {
        _userLogic.AddBookingToUser(_personRepo.GetFromRepository(_person.GetEmail()), _mybooking);
        _userLogic.RemoveBookingFromUser(_personRepo.GetFromRepository(_person.GetEmail()), _mybooking);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenSomeoneThatIsNotAUserGetsBookingRejectedShouldThrowException()
    {
        Administrator admin = new Administrator("Franco", "Ramos", "francoramos@gmail.com", "PassWord921#2");
        _userLogic.RemoveBookingFromUser(_personRepo.GetFromRepository(admin.GetEmail()), _mybooking);
    }
}