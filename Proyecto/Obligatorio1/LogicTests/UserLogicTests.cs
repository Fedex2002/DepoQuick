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
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", null);
        _userLogic.CheckIfEmailIsRegistered(_user.GetEmail());
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPasswordIsNotCorrectThrowException()
    {
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", null);
        _userRepo.AddToRepository(_user); 
        _userLogic.CheckIfPasswordIsCorrect(_user.GetPassword(), "Catch from page");
        
    }
}