using Logic;
using Model;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class LogicTests
{
    private UserLogic _userLogic = new UserLogic();
    private User _user;
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", null);
        _userLogic.CheckIfEmailIsRegistered(_user.GetEmail());
    }
}