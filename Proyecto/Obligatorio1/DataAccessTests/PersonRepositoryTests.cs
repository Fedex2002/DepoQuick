using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Exceptions;

namespace DataAccessTests;

[TestClass]
public class PersonRepositoryTests
{
    private PersonsRepository _repository;
    private ApplicationDbContext _context;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private Person _person;

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _repository = new PersonsRepository(_context);
        _person = new Person("John", "Doe", "johndoe@gmail.com", "PassWord921#", false);
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void WhenAddingANewPerson_ShouldAddTheNewPersonInPersonsTable()
    {
        _repository.AddPerson(_person);

        var personInDb = _context.Persons.First();

        Assert.AreEqual(_person, personInDb);
    }

    [TestMethod]
    public void WhenPersonExists_ShouldReturnTrue()
    {
        _repository.AddPerson(_person);

        bool exists = _repository.PersonAlreadyExists(_person);

        Assert.IsTrue(exists);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenPersonAlreadyExists_ShouldThrowRepositoryException()
    {
        _repository.AddPerson(_person);
        _repository.AddPerson(_person);
    }

    [TestMethod]
    public void WhenTryingToFindAPerson_ShouldReturnTrueIfPersonIsInTheDatabase()
    {
        _repository.AddPerson(_person);

        Person personInDb = _repository.FindPersonByEmail(_person.Email);

        Assert.AreEqual(_person, personInDb);
    }
}