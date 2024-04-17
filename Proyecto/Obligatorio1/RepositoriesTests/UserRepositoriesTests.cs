using Model;
using Repositories;

namespace RepositoriesTests;

[TestClass]
public class UserRepositoryTest
{
    private UserRepositories _userepo;
    private User _user;
    [TestInitialize] 
    public void TestInitialize()
    {
        _userepo = new UserRepositories();
        _user = new User();
    }

    [TestMethod]
    public void WhenAddingNewUserShouldAddItToRepository()
    {
        _userepo.AddUser(_user);
        User userInRepo = _userepo.FindUser(_user);
        Assert.AreEqual(_user.GetEmail(), userInRepo.GetEmail());
    }
}