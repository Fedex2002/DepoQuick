using Model;
using Model.Exceptions;
using Repositories;

namespace RepositoriesTests;

[TestClass]
public class UserRepositoryTest
{
    private UserRepositories _userepo;
    private User _user;
    private List<Booking> _bookings;
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
        User userInRepo = _userepo.GetFromRepository(_user);
        Assert.AreEqual(_user.GetEmail(), userInRepo.GetEmail());
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenAddingExistingUserShouldThrowAnException()
    {
        _user = new User("John", "Doe", "johndoe@gmail.com", "PassWord921#", _bookings);
        _userepo.AddToRepository(_user);
        _userepo.AddToRepository(_user);
    }
    
    [TestMethod] 
    public void WhenDeletingUserShouldRemoveItFromRepository()
    {
        _userepo.AddToRepository(_user);
        _userepo.RemoveFromRepository(_user);
        Assert.IsFalse(_userepo.ExistsInRepository(_user));
    }
}