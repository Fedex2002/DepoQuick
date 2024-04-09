using Model;
using Model.Exceptions;

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
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#");
        Assert.AreEqual("Franco", _myperson.GetName());
        Assert.AreEqual("Ramos", _myperson.GetSurname());
        Assert.AreEqual("francoramos1511@gmail.com", _myperson.GetEmail());
        Assert.AreEqual("FrancoRamos2023#", _myperson.GetPassword());
    }
    
    [TestMethod]
    public void WhenCreatingANewPersonWithPasswordValidations_ShouldReturnTrueIfItIsAValidPassword()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#");
        Assert.IsTrue(_myperson.ValidatePassword());
    }

    [TestMethod]
    [ExpectedException(typeof(PersonExceptions))]
    public void WhenCreatingANewPersonWithPasswordValidations_ShouldReturnExceptionIfItIsNotAValidPassword()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "franco");
    }
    
    
}