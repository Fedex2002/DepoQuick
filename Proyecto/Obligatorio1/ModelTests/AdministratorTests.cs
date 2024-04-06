namespace ModelTests;

[TestClass]
public class AdministratorTests
{
    private Administrator _myadministrator;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _myadministrator = new Administrator();
    }
    
    [TestMethod]
    public void CreatingEmptyAdministratorShouldReturnEmpty()
    {
        Assert.IsNotNull(_myadministrator);
    }
    
}