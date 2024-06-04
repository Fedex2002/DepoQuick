using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Logic;
using Repositories;
namespace LogicTests;

[TestClass]
public class SessionLogicTests
{
    private PersonsRepository _personRepo;
    private PersonLogic _personLogic;
    private Person _person;
    private SessionLogic _sessionLogic;
    private ApplicationDbContext _context;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();

    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _personRepo = new PersonsRepository(_context);
        _personLogic = new PersonLogic(_personRepo);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#",false);
        _personRepo.AddPerson(_person);
        _sessionLogic = new SessionLogic(_personLogic);
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void WhenPersonIsLoggedInSetItAsCurrentPerson()
    {
        _sessionLogic.Login(_personLogic.GetRepository().FindPersonByEmail(_person.Email).Email,_personLogic.GetRepository().FindPersonByEmail(_person.Email).Password);
    }
    
    [TestMethod]
    public void WhenPersonIsLoggedOutSetCurrentPersonToEmpty()
    {
        _sessionLogic.Logout();
    }
}