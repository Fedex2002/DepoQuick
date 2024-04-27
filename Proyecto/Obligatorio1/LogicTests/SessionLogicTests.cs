using Model;
using Logic;
using Repositories;
namespace LogicTests;

[TestClass]
public class SessionLogicTests
{
    private UserRepositories _userRepo;
    private UserLogic _userLogic;
    private User _user;
    private SessionLogic _sessionLogic;

    [TestInitialize]
    public void TestInitialize()
    {
        _userRepo = new UserRepositories();
        _userLogic = new UserLogic(_userRepo);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", new List<Booking>());
        _userRepo.AddToRepository(_user);
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