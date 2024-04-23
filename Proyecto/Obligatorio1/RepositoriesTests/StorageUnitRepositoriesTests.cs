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
    
    [TestInitialize]
    public void TestInitialize()
    {
        _storageUnitRepositories = new StorageUnitRepositories();
        _storageUnit = new StorageUnit("",AreaType.A, SizeType.Small, true,null );
    }
    
    
    [TestMethod]
    public void WhenAddingNewStorageUnitShouldAddItToRepository()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        StorageUnit storageUnitInRepo = _storageUnitRepositories.GetFromRepository(_storageUnit);
        Assert.AreEqual(_storageUnit.GetId(), storageUnitInRepo.GetId());
    }
    
    [TestMethod]
    public void WhenAStorageUnitExistsInRepositoryShouldFindIt()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        Assert.IsTrue(_storageUnitRepositories.ExistsInRepository(_storageUnit.GetId()));
    }
    
    [TestMethod]
    public void WhenDeletingStorageUnitShouldRemoveItFromRepository()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        _storageUnitRepositories.RemoveFromRepository(_storageUnit);
        Assert.IsFalse(_storageUnitRepositories.ExistsInRepository(_storageUnit.GetId()));
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenAddingTheSameStorageUnitShouldThrowAnException()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        _storageUnitRepositories.AddToRepository(_storageUnit);
    }
    
}