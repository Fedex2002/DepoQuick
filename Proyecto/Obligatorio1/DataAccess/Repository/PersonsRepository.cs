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
        if (PersonAlreadyExists(person))
        {
            PersonAlreadyExistsSoThrowException();
        }
        _database.Persons.Add(person);

        _database.SaveChanges();
    }

    private static void PersonAlreadyExistsSoThrowException()
    {
        throw new RepositoryExceptions("The person already exists");
    }

    public bool PersonAlreadyExists(Person newPerson)
    {
        return _database.Persons.Any(person => person.Email == newPerson.Email);
    }
}