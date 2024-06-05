using DataAccess.Repository;
using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class PersonLogic
{
    private readonly PersonsRepository _personRepositories;
    
    public PersonLogic(PersonsRepository personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public bool CheckIfEmailIsRegistered(string email)
    {
        return _personRepositories.PersonAlreadyExists(email);
    }

    public void IfEmailIsNotRegisteredThrowException(bool registered)
    {
        if (!registered)
            throw new LogicExceptions("The email is not registered");
    }
    
    public bool CheckIfPasswordIsCorrect(string personpass, string catchFromPage)
    {
       
        if (PasswordStringMatch(personpass, catchFromPage))
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
            throw new LogicExceptions("The email is not registered");
        }

        return personDto;
    }
    

    public PersonsRepository GetRepository()
    {
        return _personRepositories;
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
    