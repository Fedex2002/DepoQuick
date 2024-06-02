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
            throw new RepositoryExceptions("The person already exists");
        }
        _database.Persons.Add(person);

        _database.SaveChanges();
    }
    
    public bool PersonAlreadyExists(Person newPerson)
    {
        return _database.Persons.Any(person => person.Email == newPerson.Email);
    }
}