using DataAccess.Context;
using Model;

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
        _database.Persons.Add(person);

        _database.SaveChanges();
    }
}