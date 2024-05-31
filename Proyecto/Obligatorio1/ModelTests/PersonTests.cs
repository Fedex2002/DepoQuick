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
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#",false);
        Assert.AreEqual("Franco", _myperson.Name);
        Assert.AreEqual("Ramos", _myperson.Surname);
        Assert.AreEqual("francoramos1511@gmail.com", _myperson.Email);
        Assert.AreEqual("FrancoRamos2023#", _myperson.Password);
    }
    
    [TestMethod]
    public void WhenCreatingANewPersonWithPasswordValidations_ShouldReturnTrueIfItIsAValidPassword()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#",false);
        Assert.IsTrue(_myperson.ValidatePassword());
    }

    [TestMethod]
    [ExpectedException(typeof(PersonExceptions))]
    public void WhenCreatingANewPersonWithPasswordValidations_ShouldReturnExceptionIfItIsNotAValidPassword()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "franco",false);
    }
    
    [TestMethod]
    public void WhenCreatingANewPersonWithValidEmail_ShouldReturnTrueIfItIsAValidEmail()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#",false);
        Assert.IsTrue(_myperson.ValidateEmail());
    }
    
    [TestMethod]
    [ExpectedException(typeof(PersonExceptions))]
    public void WhenCreatingANewPersonWithEmailValidations_ShouldReturnExceptionIfItIsNotAValidEmail()
    {
        _myperson = new Person("Franco", "Ramos", "francoramos1511gmail.com", "franco",false);
    }
    
    [TestMethod]
    public void WhenCreatingANewPersonWithNameAndSurnameValidations_ShouldReturnTrueIfItIsAValidNameAndSurname()
    {
        _myperson = new Person("Franco Maximiliano", "Ramos Risso", "francoramos1511@gmail.com", "FrancoRamos2023#",false);
        Assert.IsTrue(_myperson.ValidateNameAndSurname());
    }
    
    [TestMethod]
    [ExpectedException(typeof(PersonExceptions))]
    public void WhenCreatingANewPersonWithNameAndSurnameValidations_ShouldReturnExceptionIfItIsNotAValidNameAndSurname()
    {
        _myperson = new Person("", "Ra2m#s", "francoramos1511gmail.com", "franco",false);
    }

    [TestMethod]

    public void WhenCreatingAPersonThatIsAnAdminShouldSetIsAdminToTrue()
    {
        _myperson = new Person("Franco Maximiliano", "Ramos Risso", "francoramos1511@gmail.com", "FrancoRamos2023#",true);
        Assert.IsTrue(_myperson.IsAdmin);
    }

}