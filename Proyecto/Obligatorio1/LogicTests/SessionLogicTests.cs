using Model;
using Logic;
using Repositories;
namespace LogicTests;

[TestClass]
public class SessionLogicTests
{
    private PersonRepositories _personRepo;
    private UserLogic _userLogic;
    private User _user;
    private SessionLogic _sessionLogic;

    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _userLogic = new UserLogic(_personRepo);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<Booking>());
        _personRepo.AddToRepository(_user);
        _sessionLogic = new SessionLogic(_userLogic);
    }

    [TestMethod]
    public void WhenUserIsLoggedInSetItAsCurrentUser()
    {
        _sessionLogic.Login(_userLogic.GetRepository().GetFromRepository(_user.GetEmail()).GetEmail(),_userLogic.GetRepository().GetFromRepository(_user.GetEmail()).GetPassword());
    }
    
    [TestMethod]
    public void WhenUserIsLoggedOutSetCurrentUserToEmpty()
    {
        _sessionLogic.Logout(_userLogic.GetRepository().GetFromRepository(_user.GetEmail()));
    }
}