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
    


    public Person Login(string email,string password)
    {
        return LoginCheckPersonValidations(email, password);
    }

    private Person LoginCheckPersonValidations(string email, string password)
    {
        Person person = new Person();
        if (CheckIfEmailIsRegistered(email) && CheckIfPasswordIsCorrect(password, _personRepositories.GetFromRepository(email).GetPassword()))
        {
            person = _personRepositories.GetFromRepository(email);
        }
        else
        {
            throw new LogicExceptions("The Person does not exist");
        }

        return person;
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