using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class PersonLogic
{
    private PersonRepositories _personRepositories;
    
    public PersonLogic(PersonRepositories personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public bool CheckIfEmailIsRegistered(string email)
    {
        return _personRepositories.ExistsInRepository(email);
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
            Person person = _personRepositories.GetFromRepository(email);
            if (CheckIfPasswordIsCorrect(password, person.GetPassword()))
            {
                personDto= new PersonDto(person.GetName(), person.GetSurname(), person.GetEmail(), person.GetPassword());
            }
        }
        else
        {
            throw new LogicExceptions("The email is not registered");
        }

        return personDto;
    }

    public PersonRepositories GetRepository()
    {
        return _personRepositories;
    }

    public void SignUp(Person person)
    {
        if (!CheckIfEmailIsRegistered(person.GetEmail()))
        {
            _personRepositories.AddToRepository(person);
        }
        else
        {
            throw new LogicExceptions("The email is already registered");
        }
    }
}