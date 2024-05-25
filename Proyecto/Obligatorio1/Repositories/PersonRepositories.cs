using RepositoriesInterface;
using Model;
using Model.Exceptions;

namespace Repositories;
public class PersonRepositories : IRepositories<Person>
{
    private readonly List<Person> _persons = new List<Person>();
    
     public void AddToRepository(Person person)
     {
         if (ExistsInRepository(person.Email))
         {
             ThrowException();
         }
         _persons.Add(person);
     }

    private static void ThrowException()
    {
        throw new RepositoryExceptions("The person already exists");
    }

    public Person GetFromRepository(string email)
    {
        Person personInRepo = _persons.Find(u => u.Email == email);
        return personInRepo;
    }

    public bool ExistsInRepository(string email)
    {
        return _persons.Any(u => u.Email == email);
    }
    public void RemoveFromRepository(Person person)
    {
        _persons.Remove(person);
    }
    
    public List<Person> GetAllFromRepository()
    {
        return _persons;
    }
  
}