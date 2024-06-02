using DataAccess.Context;
using DataAccess.Repository;
using Model;

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
}