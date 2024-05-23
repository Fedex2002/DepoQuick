using Model;
using Model.Exceptions;
using Repositories;

namespace RepositoriesTests;

[TestClass]
public class PersonRepositoryTest
{
    private PersonRepositories _personRepo;
    private Person _person;
    [TestInitialize] 
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _person = new Person();
    }

    [TestMethod]
    public void WhenAddingNewPersonShouldAddItToRepository()
    {
        _personRepo.AddToRepository(_person);
        Person personInRepo = _personRepo.GetFromRepository(_person.Email);
        Assert.AreEqual(_person.Email, personInRepo.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenAddingExistingPersonShouldThrowAnException()
    {
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#");
        _personRepo.AddToRepository(_person);
        _personRepo.AddToRepository(_person);
    }
    
    [TestMethod] 
    public void WhenDeletingPersonShouldRemoveItFromRepository()
    {
        _personRepo.AddToRepository(_person);
        _personRepo.RemoveFromRepository(_person);
        Assert.IsFalse(_personRepo.ExistsInRepository(_person.Email));
    }

    [TestMethod]

    public void WhenGettingAllPersonsFromRepositoryShouldReturnIt()
    {
        Person person2 = new Person("Jane", "Doe", "janedoe@gmail.com", "PassWord921#");
        _personRepo.AddToRepository(_person);
        _personRepo.AddToRepository(person2);
        List<Person> persons = _personRepo.GetAllFromRepository();
        Assert.AreEqual(2, persons.Count);
    }
}