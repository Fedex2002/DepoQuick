using Model;
namespace ModelTests;

[TestClass]
public class PersonTests
{
    private Person _myperson;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _myperson = new Person();
    }
    
    [TestMethod]
    public void CreatingEmptyPersonShouldReturnEmpty()
    {
        Assert.IsNotNull(_myperson);
    }
    
}