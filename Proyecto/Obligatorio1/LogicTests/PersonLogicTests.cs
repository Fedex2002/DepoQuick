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
    private PersonDto _personDto;
    private List<BookingDto> _bookingsDto;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _personLogic = new PersonLogic(_personRepo);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _personDto = new PersonDto("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _bookingsDto = new List<BookingDto>();
        _userDto = new UserDto("User", "User", "emailuser@gmail.com","PassWord921#",_bookingsDto);
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
    public void WhenEmptyPersonDtoIsCreatedShouldReturnEmptyPersonDto()
    {
        PersonDto personDto = new PersonDto();
        Assert.IsNotNull(personDto);
    }
    
    [TestMethod]
    public void WhenEmptyUserDtoIsCreatedShouldReturnEmptyUserDto()
    {
        UserDto userDto = new UserDto();
        Assert.IsNotNull(userDto);
    }
    
    [TestMethod]
    public void WhenEmptyAdministratorDtoIsCreatedShouldReturnEmptyAdministratorDto()
    {
        AdministratorDto administratorDto = new AdministratorDto();
        Assert.IsNotNull(administratorDto);
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
        User user = new User("User", "User", "emailuser@gmail.com","PassWord921#", new List<Booking>());
        _personRepo.AddToRepository(user);
        PersonDto loggedInPersonDto = _personLogic.Login(user.GetEmail(),user.GetPassword());
        
        Assert.AreEqual(user.GetName(), loggedInPersonDto.Name);
        Assert.AreEqual(user.GetSurname(), loggedInPersonDto.Surname);
        Assert.AreEqual(user.GetEmail(), loggedInPersonDto.Email);
        Assert.AreEqual(user.GetPassword(), loggedInPersonDto.Password);
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
        Booking booking = new Booking(false, new DateTime(2024, 7, 1), new DateTime(2024, 8, 15), new StorageUnit("", AreaType.A, SizeType.Small, true,promotions ), "");
        user.GetBookings().Add(booking);
        List<BookingDto> bookingsDtos = _personLogic.ChangeToBookingsDtos(user.GetBookings());
        Assert.AreEqual(user.GetBookings().Count, bookingsDtos.Count);
    }
}