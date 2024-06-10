using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Enums;
using Model.Exceptions;

namespace DataAccessTests;

[TestClass]
public class StorageUnitsRepositoryTests
{
    private StorageUnitsRepository _repository;
    private ApplicationDbContext _context;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private StorageUnit _storageUnit;

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _repository = new StorageUnitsRepository(_context);
        _storageUnit = new StorageUnit("12", AreaType.A, SizeType.Small, true, new List<Promotion>(),
            new List<DateRange>());
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void WhenAddingANewStorageUnit_ShouldAddTheNewStorageUnitInStorageUnitsTable()
    {
        _repository.AddStorageUnit(_storageUnit);

        var storageUnitInDb = _context.StorageUnits.First();

        Assert.AreEqual(_storageUnit, storageUnitInDb);
    }
    
    [TestMethod]
    public void WhenStorageUnitExists_ShouldReturnTrue()
    {
        _repository.AddStorageUnit(_storageUnit);

        bool exists = _repository.StorageUnitAlreadyExists(_storageUnit);

        Assert.IsTrue(exists);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenStorageUnitAlreadyExists_ShouldThrowRepositoryException()
    {
        _repository.AddStorageUnit(_storageUnit);
        _repository.AddStorageUnit(_storageUnit);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenStorageUnitDoesNotExist_ShouldThrowRepositoryException()
    {
        _repository.DeleteStorageUnit(_storageUnit);
    }
    
    [TestMethod]
    public void WhenTryingToFindAStorageUnit_ShouldReturnTheStorageUnitIfItIsInTheDatabase()
    {
        _repository.AddStorageUnit(_storageUnit);

        StorageUnit storageUnitInDb = _repository.GetStorageUnitFromId(_storageUnit.Id);

        Assert.AreEqual(_storageUnit, storageUnitInDb);
    }
    
    [TestMethod]
    public void WhenGettingAllStorageUnits_ShouldReturnAllTheStorageUnitsInTheDatabase()
    {
        _repository.AddStorageUnit(_storageUnit);

        List<StorageUnit> storageUnitsInDb = _repository.GetAllStorageUnits();

        Assert.AreEqual(1, storageUnitsInDb.Count);
    }
    
    [TestMethod]
    public void WhenRemovingAStorageUnit_ShouldRemoveTheStorageUnitFromTheDatabase()
    {
        _repository.AddStorageUnit(_storageUnit);

        _repository.DeleteStorageUnit(_storageUnit);

        Assert.AreEqual(0, _context.StorageUnits.Count());
    }

    
    [TestMethod]
    
    public void WhenAddingAValidDateRangeToStorageUnit_ShouldAddTheDateRangeToTheStorageUnit()

    {
        _repository.AddStorageUnit(_storageUnit);
        DateRange dateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(1));
        _repository.AddAvailableDateToStorageUnit(_storageUnit.Id, dateRange);
        StorageUnit storageUnitInDb = _repository.GetStorageUnitFromId(_storageUnit.Id);
        Assert.AreEqual(1, storageUnitInDb.AvailableDates.Count);

    }

    
    [TestMethod]
    
    public void WhenDeletingAValidDateRangeFromStorageUnit_ShouldDeleteTheDateRangeFromTheStorageUnit()

    {
        _repository.AddStorageUnit(_storageUnit);
        DateRange dateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(1));
        _repository.AddAvailableDateToStorageUnit(_storageUnit.Id, dateRange);
        StorageUnit storageUnitInDb = _repository.GetStorageUnitFromId(_storageUnit.Id);
        Assert.AreEqual(1, storageUnitInDb.AvailableDates.Count);
        _repository.DeleteAvailableDateFromStorageUnit(_storageUnit.Id, dateRange);
        storageUnitInDb = _repository.GetStorageUnitFromId(_storageUnit.Id);
        Assert.AreEqual(0, storageUnitInDb.AvailableDates.Count);

    }

}