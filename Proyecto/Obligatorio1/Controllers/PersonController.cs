using DataAccess.Repository;
using Logic.DTOs;
using Logic.Interfaces;
using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class PersonController : IPersonController
{
    public PersonDto CurrentPerson { get; set; }
    private readonly PersonsRepository _personRepositories;
    
    public PersonController(PersonsRepository personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public bool CheckIfEmailIsRegistered(string email)
    {
        return _personRepositories.PersonAlreadyExists(email);
    }

    private void IfEmailIsNotRegisteredThrowException()
    {
        throw new LogicExceptions("The email is not registered");
    }
    
    public bool CheckIfPasswordIsCorrect(string password, string verifyPassword)
    {
        if (PasswordStringMatch(password, verifyPassword))
            throw new LogicExceptions("The password is not correct");
        return true;
    }

    private static bool PasswordStringMatch(string personpass, string catchFromPage)
    {
        return personpass != catchFromPage;
    }
    
    public PersonDto Login(string email, string password)
    {
        return LoginCheckPersonValidations(email, password);
    }

    private PersonDto LoginCheckPersonValidations(string email, string password)
    {
        PersonDto personDto = new PersonDto();
        if (CheckIfEmailIsRegistered(email))
        {
            Person person = _personRepositories.FindPersonByEmail(email);
            if (CheckIfPasswordIsCorrect(password, person.Password))
            {
                personDto = new PersonDto(person.Name, person.Surname, person.Email, person.Password,person.IsAdmin);
            }
        }
        else
        {
            IfEmailIsNotRegisteredThrowException();
        }

        CurrentPerson = personDto;

        return personDto;
    }
    
    public void Logout()
    {
        CurrentPerson = null;
    }

    public void SignUp(PersonDto personDto)
    {
        if (!CheckIfEmailIsRegistered(personDto.Email))
        {
            AddPersonIfItsValid(personDto);
        }
        else
        {
            throw new LogicExceptions("The email is already registered");
        }
    }

    private void AddPersonIfItsValid(PersonDto personDto)
    {
        Person personToRepo = new Person(personDto.Name, personDto.Surname,personDto.Email, personDto.Password, personDto.IsAdmin);
        _personRepositories.AddPerson(personToRepo);
        
    }

    public PersonDto GetPersonDtoFromEmail(string personEmail)
    {
        Person person = _personRepositories.FindPersonByEmail(personEmail);
        PersonDto personDto = new PersonDto(person.Name, person.Surname, person.Email, person.Password, person.IsAdmin);
        return personDto;
    }
}
    