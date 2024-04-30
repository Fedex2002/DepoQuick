using Logic;
using Logic.DTOs;
using Model;
using Model.Enums;
using Model.Exceptions;
using Repositories;


namespace LogicTests;

[TestClass]
public class PersonLogicTests
{
    private PersonRepositories _personRepo;
    private PersonLogic _personLogic;
    private Person _person;
    private UserDto _userDto;
    private User _user;
    private PersonDto _personDto;
    private List<Booking> _bookings;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _personLogic = new PersonLogic(_personRepo);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _personDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _bookings = new List<Booking>();
        _user = new User("User", "User", "emailuser@gmail.com","PassWord921#",_bookings);
        _userDto = new UserDto("User", "User", "emailuser@gmail.com","PassWord921#",_bookings);
        _personRepo.AddToRepository(_person); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _personRepo.RemoveFromRepository(_person);;
        _personLogic.IfEmailIsNotRegisteredThrowException(_personLogic.CheckIfEmailIsRegistered(_person.GetEmail()));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPasswordIsNotCorrectThrowException()
    {
        _personRepo.RemoveFromRepository(_person);
        _personLogic.CheckIfPasswordIsCorrect(_person.GetPassword(), "Catch from page");
    }

    [TestMethod]
    public void WhenEmailIsRegisteredReturnTrue()
    {
        Assert.IsTrue(_personLogic.CheckIfEmailIsRegistered(_person.GetEmail()));
    }

    [TestMethod]
    public void WhenPasswordIsCorrectReturnTrue()
    {
        Assert.IsTrue(_personLogic.CheckIfPasswordIsCorrect(_person.GetPassword(), _person.GetPassword()));
    }
    
    [TestMethod]
    public void WhenPersonIsTryingToLoginShouldReturnPersonIfValidationsAreCorrect()
    {
        PersonDto loggedInPersonDto = _personLogic.Login(_person.GetEmail(), _person.GetPassword());
        PersonDto expectedPersonDto = new PersonDto(_person.GetName(), _person.GetSurname(), _person.GetEmail(), _person.GetPassword());
        Assert.AreEqual(expectedPersonDto.Name, loggedInPersonDto.Name);
        Assert.AreEqual(expectedPersonDto.Surname, loggedInPersonDto.Surname);
        Assert.AreEqual(expectedPersonDto.Email, loggedInPersonDto.Email);
        Assert.AreEqual(expectedPersonDto.Password, loggedInPersonDto.Password);
    }
    
    [TestMethod]
    public void WhenPersonIsTryingToLoginAndIsAdministratorShouldReturnAdministrator()
    {
        Administrator admin = new Administrator("Admin", "Admin","email@gmail.com","PassWord921#EAa");
        _personRepo.AddToRepository(admin);
        PersonDto loggedInAdministratorDto = _personLogic.Login(admin.GetEmail(),admin.GetPassword());
        Assert.AreEqual(admin.GetName(), loggedInAdministratorDto.Name);
        Assert.AreEqual(admin.GetSurname(), loggedInAdministratorDto.Surname);
        Assert.AreEqual(admin.GetEmail(), loggedInAdministratorDto.Email);
        Assert.AreEqual(admin.GetPassword(), loggedInAdministratorDto.Password);
    }

    [TestMethod]
    public void WhenPersonIsTryingToLoginAndIsUserShouldReturnUser()
    {
        _personRepo.AddToRepository(_user);
        PersonDto loggedInUserDto = _personLogic.Login(_user.GetEmail(),_user.GetPassword());
        Assert.AreEqual(_user.GetName(), loggedInUserDto.Name);
        Assert.AreEqual(_user.GetSurname(), loggedInUserDto.Surname);
        Assert.AreEqual(_user.GetEmail(), loggedInUserDto.Email);
        Assert.AreEqual(_user.GetPassword(), loggedInUserDto.Password);
        Assert.AreEqual(_user.GetBookings(), loggedInUserDto.Bookings);
    }


    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPersonIsTryingToLoginAndDoesNotExistShouldReturnException()
    {
        Assert.AreEqual(_person, _personLogic.Login("mail@gmail.com", "PassWord921#EAa"));
    }

    [TestMethod]
    public void WhenPersonsAreAddedToRepositoryShouldReturnTheRepository()
    {
        Person federico = new Person("Fede", "Ramos", "FedeRamos@gmail.com", "PaSSWorD921#");
        _personRepo.AddToRepository(federico); 
        Assert.AreEqual(_personRepo, _personLogic.GetRepository());
    }

    [TestMethod]
    public void WhenPersonIsTryingToSignUpShouldAddPersonToRepositoryIfValidationsAreCorrect()
    {
        _personRepo.RemoveFromRepository(_person);
        _personLogic.SignUp(_personDto);
        Assert.IsTrue(_personRepo.ExistsInRepository(_person.GetEmail()));
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPersonIsTryingToSignUpAndEmailIsAlreadyRegisteredShouldReturnException()
    {
        _personLogic.SignUp(_personDto);
    }
}