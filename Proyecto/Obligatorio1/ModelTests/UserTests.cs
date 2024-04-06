namespace ModelTests;

[TestClass]
public class UserTests
{
    private User _myuser;

    [TestInitialize]
    public void TestInitialize()
    {
        _myuser = new User();
    }

    [TestMethod]
    public void CreatingEmptyUserShouldReturnEmpty()
    {
        Assert.IsNotNull(_myuser);
    }
}