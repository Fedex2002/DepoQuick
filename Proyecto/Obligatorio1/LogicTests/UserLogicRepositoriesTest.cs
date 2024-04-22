using Repositories;

namespace LogicTests;

[TestClass]
public class UserLogicRepositoriesTest
{
    [TestInitialize]
    public void TestInitialize()
    {
        _users = new UserRepositories();
    }
    private UserRepositories _users = new UserRepositories();
    
    [TestMethod]
    


}