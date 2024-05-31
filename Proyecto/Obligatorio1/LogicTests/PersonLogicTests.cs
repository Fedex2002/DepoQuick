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
    private PersonDto _personDto;
    private List<BookingDto> _bookingsDto;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _personLogic = new PersonLogic(_personRepo);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#",false);
        _personDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _bookingsDto = new List<BookingDto>();
        _personRepo.AddToRepository(_person); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenEmailIsNotRegisteredThrowException()
    {
        _personRepo.RemoveFromRepository(_person);;
        _personLogic.IfEmailIsNotRegisteredThrowException(_personLogic.CheckIfEmailIsRegistered(_person.Email));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPasswordIsNotCorrectThrowException()
    {
        _personRepo.RemoveFromRepository(_person);
        _personLogic.CheckIfPasswordIsCorrect(_person.Password, "Catch from page");
    }

    [TestMethod]
    public void WhenEmailIsRegisteredReturnTrue()
    {
        Assert.IsTrue(_personLogic.CheckIfEmailIsRegistered(_person.Email));
    }

    [TestMethod]
    public void WhenPasswordIsCorrectReturnTrue()
    {
        Assert.IsTrue(_personLogic.CheckIfPasswordIsCorrect(_person.Password, _person.Password));
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
        PersonDto loggedInPersonDto = _personLogic.Login(_person.Email, _person.Password);
        PersonDto expectedPersonDto = new PersonDto(_person.Name, _person.Surname, _person.Email, _person.Password);
        Assert.AreEqual(expectedPersonDto.Name, loggedInPersonDto.Name);
        Assert.AreEqual(expectedPersonDto.Surname, loggedInPersonDto.Surname);
        Assert.AreEqual(expectedPersonDto.Email, loggedInPersonDto.Email);
        Assert.AreEqual(expectedPersonDto.Password, loggedInPersonDto.Password);
    }
    
    [TestMethod]
    public void WhenPersonIsTryingToLoginAndIsAdministratorShouldReturnPersonWithAdminPrivileges()
    {
        _person.IsAdmin = true;
        _personRepo.AddToRepository(_person);
        PersonDto loggedInAdministratorDto = _personLogic.Login(_person.Email,_person.Password);
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
        _personRepo.AddToRepository(_person);
        PersonDto loggedInPersonDto = _personLogic.Login(_person.Email,_person.Password);
        
        Assert.AreEqual(_person.Name, loggedInPersonDto.Name);
        Assert.AreEqual(_person.Surname, loggedInPersonDto.Surname);
        Assert.AreEqual(_person.Email, loggedInPersonDto.Email);
        Assert.AreEqual(_person.Password, loggedInPersonDto.Password);
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
    public void WhenUserIsTryingToSignUpShouldAddUserToRepositoryIfValidationsAreCorrect()
    {
        PersonDto userDto = new UserDto("John", "Doe", "johndoe123@gmail.com", "PassWord921#",_bookingsDto);
        _personLogic.SignUp(userDto);
        Assert.IsTrue(_personRepo.ExistsInRepository(userDto.Email));
    }
    
    [TestMethod]
    public void WhenAdministratorIsTryingToSignUpShouldAddAdministratorToRepositoryIfValidationsAreCorrect()
    {
        PersonDto adminDto = new AdministratorDto("John", "Doe", "johndoe1235@gmail.com", "PassWord921#");
        _personLogic.SignUp(adminDto);
        Assert.IsTrue(_personRepo.ExistsInRepository(adminDto.Email));

    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPersonIsTryingToSignUpAndEmailIsAlreadyRegisteredShouldReturnException()
    {
        _personLogic.SignUp(_personDto);
    }
    
    [TestMethod]
    public void WhenUserIsTryingToLoginInShouldReturnAListOfHisBookingDtos()
    {
        User user = new User("Franco", "Ramos", "francoramos1511@gmail.com", "PassWord921#2", new List<Booking>());
        _personRepo.AddToRepository(user);
        List<Promotion> promotions = new List<Promotion>();
        Promotion promotion = new Promotion("Promo", 10, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15));
        promotions.Add(promotion);
        List<DateRange> availableDates = new List<DateRange>();
        availableDates.Add(new DateRange(new DateTime(2024, 7, 1), new DateTime(2024, 8, 15)));
        Booking booking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), new StorageUnit("", AreaType.A, SizeType.Small, true,promotions, availableDates), "", "Reservado", false);
        user.Bookings.Add(booking);
        List<BookingDto> bookingsDtos = _personLogic.ChangeToBookingsDtos(user.Bookings);
        Assert.AreEqual(user.Bookings.Count, bookingsDtos.Count);
    }
}