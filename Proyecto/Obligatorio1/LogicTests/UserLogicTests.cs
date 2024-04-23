using Logic;
using Model;
using Model.Exceptions;
using Repositories;

namespace LogicTests;

[TestClass]
public class LogicTests
{
    private UserRepositories _userRepo;
    private UserLogic _userLogic;
    private User _user;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _userRepo = new UserRepositories();
        _userLogic = new UserLogic(_userRepo);
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _userLogic.CheckIfEmailIsRegistered(_user.GetEmail());
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPasswordIsNotCorrectThrowException()
    {
        _userRepo.AddToRepository(_user); 
        _userLogic.CheckIfPasswordIsCorrect(_user.GetPassword(), "Catch from page");
    }

    [TestMethod]
    public void WhenEmailIsRegisteredReturnTrue()
    {
        _userRepo.AddToRepository(_user);
        Assert.IsTrue(_userLogic.CheckIfEmailIsRegistered(_user.GetEmail()));
    }

    [TestMethod]
    public void WhenPasswordIsCorrectReturnTrue()
    {
        _userRepo.AddToRepository(_user);
        Assert.IsTrue(_userLogic.CheckIfPasswordIsCorrect(_user.GetPassword(), _user.GetPassword()));
    }
}