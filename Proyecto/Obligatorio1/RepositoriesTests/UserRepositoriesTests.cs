using Model;
using Model.Exceptions;
using Repositories;

namespace RepositoriesTests;

[TestClass]
public class UserRepositoryTest
{
    private UserRepositories _userepo;
    private User _user;
    private Booking _booking;
    [TestInitialize] 
    public void TestInitialize()
    {
        _userepo = new UserRepositories();
        _user = new User();
    }

    [TestMethod]
    public void WhenAddingNewUserShouldAddItToRepository()
    {
        _userepo.AddToRepository(_user);
        User userInRepo = _userepo.FindUser(_user);
        Assert.AreEqual(_user.GetEmail(), userInRepo.GetEmail());
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenAddingExistingUserShouldThrowAnException()
    {
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _booking);
        _userepo.AddToRepository(_user);
        _userepo.AddToRepository(_user);
    }
}