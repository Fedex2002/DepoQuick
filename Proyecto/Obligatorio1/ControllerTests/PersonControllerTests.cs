using Controllers;
using DataAccess.Context;
using DataAccess.Repository;
using Logic;
using Logic.DTOs;
using Model;

namespace ControllerTests;

[TestClass]
public class PersonControllerTests
{
    private PersonController _personController;
    private PersonLogic _personLogic;
    private PersonsRepository _personsRepository;
    private ApplicationDbContext _context;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private Person _person;
    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _personsRepository = new PersonsRepository(_context);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
        _personLogic = new PersonLogic(_personsRepository);
        _personController = new PersonController(_personLogic);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void WhenCreatingAPersonControllerCantBeNull()
    {
        Assert.IsNotNull(_personController);
    }

    [TestMethod]

    public void WhenLoggingInWithCorrectEmailAndPasswordReturnPersonDto()
    {
        _personsRepository.AddPerson(_person);
        PersonDto personDto = _personController.Login(_person.Email, _person.Password);
        Assert.AreEqual(_person.Name, personDto.Name);
        Assert.AreEqual(_person.Surname, personDto.Surname);
        Assert.AreEqual(_person.Email, personDto.Email);
        Assert.AreEqual(_person.Password, personDto.Password);
        Assert.AreEqual(_person.IsAdmin, personDto.IsAdmin);
    }
    
    [TestMethod]
    
    public void WhenSigningUpWithCorrectDataReturnPersonDto()
    {
        PersonDto personDto = _personController.SignUp(_person.Name, _person.Surname, _person.Email, _person.Password, _person.IsAdmin);
        Assert.AreEqual(_person.Name, personDto.Name);
        Assert.AreEqual(_person.Surname, personDto.Surname);
        Assert.AreEqual(_person.Email, personDto.Email);
        Assert.AreEqual(_person.Password, personDto.Password);
        Assert.AreEqual(_person.IsAdmin, personDto.IsAdmin);
    }
}