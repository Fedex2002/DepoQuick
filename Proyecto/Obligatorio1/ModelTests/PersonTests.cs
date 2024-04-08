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
    
    [TestMethod]
    public void CreatingPersonWithValuesShouldReturnValues()
    {
        string name = "Franco";
        string surname = "Ramos";
        string email = "francoramos1511@gmail.com";
        string password = "1234";
        _myperson = new Person(name, surname, email, password);
        Assert.AreEqual("Franco", _myperson.GetName());
        Assert.AreEqual("Ramos", _myperson.GetSurname());
        Assert.AreEqual("francoramos1511@gmail.com", _myperson.GetEmail());
        Assert.AreEqual("1234", _myperson.GetPassword());
    }
    
    [TestMethod]
    public void WhenCreatingANewUserWithPasswordValidations_ShouldReturnTrueIfItIsAValidPassword()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#");
        Assert.IsTrue(_myperson.ValidatePassword());
    }
    
    
}