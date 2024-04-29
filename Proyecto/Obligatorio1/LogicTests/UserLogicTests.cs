using System.Runtime.InteropServices.ComTypes;
using Logic;
using Repositories;
using Model;
using Model.Enums;

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
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _storageUnit, "");
    }
    
    [TestMethod]
    public void WhenUserMakesABookingShouldAddItToHisListOfBookings()
    {
        _userLogic.AddBookingToUser(_personRepo.GetFromRepository(_person.GetEmail()), _mybooking);
    }
}