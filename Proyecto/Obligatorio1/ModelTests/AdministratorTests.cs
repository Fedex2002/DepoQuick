using Model;
namespace ModelTests;

[TestClass]
public class AdministratorTests
{
    private Administrator _myadministrator;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _myadministrator = new Administrator("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#");
    }
    
    [TestMethod]
    public void CreatingEmptyAdministratorShouldReturnEmpty()
    {
        _myadministrator = new Administrator();
        Assert.IsNotNull(_myadministrator);
    }

    [TestMethod]
    public void CreatingAdministratorWithValidations_ShouldReturnValues()
    {
        Assert.AreEqual("Franco", _myadministrator.Name);
        Assert.AreEqual("Ramos", _myadministrator.Surname);
        Assert.AreEqual("francoramos1511@gmail.com", _myadministrator.Email);
        Assert.AreEqual("FrancoRamos2023#", _myadministrator.Password);
    }
    
}