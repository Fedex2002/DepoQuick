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
    }

    [TestMethod]
    public void WhenUserIsLoggedInSetItAsCurrentUser()
    {
        SessionLogic.Login(_userLogic.GetRepository().GetFromRepository(_user));
    }
}