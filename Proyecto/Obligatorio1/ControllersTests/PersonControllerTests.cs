using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Enums;
using Model.Exceptions;
using Repositories;
using Controllers;
using Controllers.Dtos;


namespace LogicTests;

[TestClass]
public class PersonControllerTests
{
    private ApplicationDbContext _context;
    private PersonController _personController;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private PersonsRepository _personRepo;
    private Person _person;
    private PersonDto _personDto;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _personRepo = new PersonsRepository(_context);
        _personController = new PersonController(_context);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#",false);
        _personDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#",_person.IsAdmin);
    }
    
     [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _personController.Login(_person.Email, _person.Password);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPasswordIsNotCorrectThrowException()
    {
        _personRepo.AddPerson(_person); 
        _personController.CheckIfPasswordIsCorrect(_person.Password, "Catch from page");
    }

    [TestMethod]
    public void WhenEmailIsRegisteredReturnTrue()
    {
        _personRepo.AddPerson(_person); 
        Assert.IsTrue(_personController.CheckIfEmailIsRegistered(_person.Email));
    }

    [TestMethod]
    public void WhenPasswordIsCorrectReturnTrue()
    {
        Assert.IsTrue(_personController.CheckIfPasswordIsCorrect(_person.Password, _person.Password));
    }

    [TestMethod]
    public void WhenEmptyPersonDtoIsCreatedShouldReturnEmptyPersonDto()
    {
        PersonDto personDto = new PersonDto();
        Assert.IsNotNull(personDto);
    }
    
    [TestMethod]
    public void WhenPersonIsTryingToLoginShouldReturnPersonIfValidationsAreCorrect()
    {
        _personRepo.AddPerson(_person); 
        PersonDto loggedInPersonDto = _personController.Login(_person.Email, _person.Password);
        PersonDto expectedPersonDto = new PersonDto(_person.Name, _person.Surname, _person.Email, _person.Password, _person.IsAdmin);
        Assert.AreEqual(expectedPersonDto.Name, loggedInPersonDto.Name);
        Assert.AreEqual(expectedPersonDto.Surname, loggedInPersonDto.Surname);
        Assert.AreEqual(expectedPersonDto.Email, loggedInPersonDto.Email);
        Assert.AreEqual(expectedPersonDto.Password, loggedInPersonDto.Password);
    }
    
    [TestMethod]
    public void WhenPersonIsTryingToLoginAndIsAdministratorShouldReturnPersonWithAdminPrivileges()
    {
        _person.IsAdmin = true;
        _personRepo.AddPerson(_person); 
        PersonDto loggedInAdministratorDto = _personController.Login(_person.Email,_person.Password);
        Assert.AreEqual(_person.Name, loggedInAdministratorDto.Name);
        Assert.AreEqual(_person.Surname, loggedInAdministratorDto.Surname);
        Assert.AreEqual(_person.Email, loggedInAdministratorDto.Email);
        Assert.AreEqual(_person.Password, loggedInAdministratorDto.Password);
        Assert.AreEqual(_person.IsAdmin, loggedInAdministratorDto.IsAdmin);
    }

    [TestMethod]
    public void WhenPersonIsTryingToLoginAndIsUserShouldReturnPersonWithoutAdminPrivileges()
    {
        _person.IsAdmin = false;
        _personRepo.AddPerson(_person); 
        PersonDto loggedInPersonDto = _personController.Login(_person.Email,_person.Password);
        
        Assert.AreEqual(_person.Name, loggedInPersonDto.Name);
        Assert.AreEqual(_person.Surname, loggedInPersonDto.Surname);
        Assert.AreEqual(_person.Email, loggedInPersonDto.Email);
        Assert.AreEqual(_person.Password, loggedInPersonDto.Password);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPersonIsTryingToLoginAndDoesNotExistShouldReturnException()
    {
        Assert.AreEqual(_person, _personController.Login("mail@gmail.com", "PassWord921#EAa"));
    }
    
    [TestMethod]
    public void WhenPersonIsTryingToSignupAndIsValidShouldAddToTheRepository()
    {
        PersonDto personDto = new PersonDto(_person.Name, _person.Surname, _person.Email, _person.Password, _person.IsAdmin);
        _personController.SignUp(personDto);
    }
    
    [TestMethod]
    
    public void WhenGettingPersonDtoFromEmailShouldReturnIt()
    {
        _personRepo.AddPerson(_person); 
        PersonDto personDto = _personController.GetPersonDtoFromEmail(_person.Email);
        Assert.AreEqual(_personDto.Name, personDto.Name);
        Assert.AreEqual(_personDto.Surname, personDto.Surname);
        Assert.AreEqual(_personDto.Email, personDto.Email);
        Assert.AreEqual(_personDto.Password, personDto.Password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPersonIsTryingToSignUpAndEmailIsAlreadyRegisteredShouldReturnException()
    {
        _personRepo.AddPerson(_person); 
        _personController.SignUp(_personDto);
    }

    
    [TestMethod]
    public void WhenAdminIsRegisteredShouldReturnTrue()
    {
        _person.IsAdmin = true;
        _personRepo.AddPerson(_person); 
        Assert.IsTrue(_personController.CheckIfAdminExists());
    }

}