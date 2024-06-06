using DataAccess.Context;
using Model;
using Model.Exceptions;

namespace DataAccess.Repository;

public class PersonsRepository
{
    private ApplicationDbContext _database;
    
    public PersonsRepository(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void AddPerson(Person person)
    {
        if (PersonAlreadyExists(person.Email))
        {
            PersonAlreadyExistsSoThrowException();
        }

        AddNewPersonToPersonsTable(person);
    }

    private void AddNewPersonToPersonsTable(Person person)
    {
        _database.Persons.Add(person);

        _database.SaveChanges();
    }

    private static void PersonAlreadyExistsSoThrowException()
    {
        throw new RepositoryExceptions("The person already exists");
    }

    public bool PersonAlreadyExists(string email)
    {
        return _database.Persons.Any(person => person.Email == email);
    }
    
    public Person FindPersonByEmail(string email)
    {
        Person person = _database.Persons.FirstOrDefault(pers => pers.Email == email);
        return person;
    }
    
    public List<Person> GetAllPersons()
    {
        return _database.Persons.ToList();
    }
}