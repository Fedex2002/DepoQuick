using Model;
using Logic;
using Repositories;
namespace LogicTests;

[TestClass]
public class SessionLogicTests
{
    private PersonRepositories _personRepo;
    private PersonLogic _personLogic;
    private Person _person;
    private SessionLogic _sessionLogic;

    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _personLogic = new PersonLogic(_personRepo);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _personRepo.AddToRepository(_person);
        _sessionLogic = new SessionLogic(_personLogic);
    }

    [TestMethod]
    public void WhenPersonIsLoggedInSetItAsCurrentPerson()
    {
        _sessionLogic.Login(_personLogic.GetRepository().GetFromRepository(_person.Email).Email,_personLogic.GetRepository().GetFromRepository(_person.Email).Password);
    }
    
    [TestMethod]
    public void WhenPersonIsLoggedOutSetCurrentPersonToEmpty()
    {
        _sessionLogic.Logout();
    }
}