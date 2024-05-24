using Model;
using Model.Enums;
using Model.Exceptions;
using Repositories;

namespace RepositoriesTests;

[TestClass]

public class StorageUnitRepositoriesTests
{
    private StorageUnitRepositories _storageUnitRepositories;
    private StorageUnit _storageUnit;
    private List<DateRange> _availableDates;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _storageUnitRepositories = new StorageUnitRepositories();
        _availableDates = new List<DateRange>();
        _storageUnit = new StorageUnit("1",AreaType.A, SizeType.Small, true,null, _availableDates);
    }
    
    
    [TestMethod]
    public void WhenAddingNewStorageUnitShouldAddItToRepository()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        StorageUnit storageUnitInRepo = _storageUnitRepositories.GetFromRepository(_storageUnit.Id);
        Assert.AreEqual(_storageUnit.Id, storageUnitInRepo.Id);
    }
    
    [TestMethod]
    public void WhenAStorageUnitExistsInRepositoryShouldFindIt()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        Assert.IsTrue(_storageUnitRepositories.ExistsInRepository(_storageUnit.Id));
    }
    
    [TestMethod]
    public void WhenDeletingStorageUnitShouldRemoveItFromRepository()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        _storageUnitRepositories.RemoveFromRepository(_storageUnit);
        Assert.IsFalse(_storageUnitRepositories.ExistsInRepository(_storageUnit.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenAddingTheSameStorageUnitShouldThrowAnException()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        _storageUnitRepositories.AddToRepository(_storageUnit);
    }
    
    [TestMethod]
    public void WhenGettingAllStorageUnitsShouldReturnAllStorageUnits()
    {
        StorageUnit storageUnit2 = new StorageUnit("2", AreaType.B, SizeType.Medium, true, null, _availableDates);
        _storageUnitRepositories.AddToRepository(_storageUnit);
        _storageUnitRepositories.AddToRepository(storageUnit2);
        List<StorageUnit> storageUnits = _storageUnitRepositories.GetAllFromRepository();
        Assert.AreEqual(2, storageUnits.Count);
    }
    
}