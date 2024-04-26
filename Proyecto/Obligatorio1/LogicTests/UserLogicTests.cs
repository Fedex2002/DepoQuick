using Logic;
using Model;
using Model.Enums;
using Model.Exceptions;
using Repositories;


namespace LogicTests;

[TestClass]
public class UserLogicTests
{
    private UserRepositories _userRepo;
    private UserLogic _userLogic;
    private User _user;
    private List<Promotion> _promotions;
    private Promotion _mypromotion;
    private StorageUnit _mystorageunit;
    private Booking _mybooking;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _userRepo = new UserRepositories();
        _userLogic = new UserLogic(_userRepo);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<Booking>());
        _userRepo.AddToRepository(_user); 
        _promotions = new List<Promotion>();
        _mypromotion= new Promotion("Winter discount", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        _promotions.Add(_mypromotion);
        _mystorageunit= new StorageUnit("20",AreaType.A, SizeType.Small, true, _promotions);
        _mybooking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, "Rejected");
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _userRepo.RemoveFromRepository(_user);;
        _userLogic.IfEmailIsNotRegisteredThrowException(_userLogic.CheckIfEmailIsRegistered(_user.GetEmail()));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPasswordIsNotCorrectThrowException()
    {
        _userRepo.RemoveFromRepository(_user);
        _userLogic.CheckIfPasswordIsCorrect(_user.GetPassword(), "Catch from page");
    }

    [TestMethod]
    public void WhenEmailIsRegisteredReturnTrue()
    {
        Assert.IsTrue(_userLogic.CheckIfEmailIsRegistered(_user.GetEmail()));
    }

    [TestMethod]
    public void WhenPasswordIsCorrectReturnTrue()
    {
        Assert.IsTrue(_userLogic.CheckIfPasswordIsCorrect(_user.GetPassword(), _user.GetPassword()));
    }

    [TestMethod]
    public void WhenUserMakesABookingShouldAddItToHisListOfBookings()
    {
        _userLogic.AddBookingToUser(_userRepo.GetFromRepository(_user.GetEmail()), _mybooking);
    }

    [TestMethod]
    public void WhenUserBookingIsApprovedShouldReturnTrue()
    {
        _userRepo.GetFromRepository(_user.GetEmail()); 
        _mybooking = new Booking(true, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), _mystorageunit, "Rejected");
        _userLogic.AddBookingToUser(_userRepo.GetFromRepository(_user.GetEmail()), _mybooking);
        bool status = _userLogic.ApprovedBooking(_userRepo.GetFromRepository(_user.GetEmail()).GetBookings().Find(Booking => Booking == _mybooking));
        Assert.IsTrue(status);
    }

    [TestMethod]
    public void WhenUserBookingIsRejectedShouldEliminateBookingFromUserListOfBookings()
    {
        _userRepo.GetFromRepository(_user.GetEmail()); 
        _userLogic.AddBookingToUser(_userRepo.GetFromRepository(_user.GetEmail()), _mybooking);
        _userLogic.RemoveBookingFromUser(_userRepo.GetFromRepository(_user.GetEmail()), _mybooking);
        
    }
    
    [TestMethod]
    public void WhenUserIsTryingToLoginShouldReturnUserIfValidationsAreCorrect()
    {
        Assert.AreEqual(_user, _userLogic.Login(_user.GetEmail(), _user.GetPassword()));
    }

    [TestMethod]
    public void WhenUsersAreAddedToRepositoryShouldReturnTheRepository()
    {
        User federico = new User("Fede", "Ramos", "FedeRamos@gmail.com", "PaSSWorD921#", new List<Booking>());
        _userRepo.AddToRepository(federico); 
        Assert.AreEqual(_userRepo, _userLogic.GetRepository());
    }

    [TestMethod]
    public void WhenUserIsTryingToSignUpShouldAddUserToRepositoryIfValidationsAreCorrect()
    {
        _userRepo.RemoveFromRepository(_user);
        _userLogic.SignUp(_user);
        Assert.IsTrue(_userRepo.ExistsInRepository(_user.GetEmail()));
    }
}