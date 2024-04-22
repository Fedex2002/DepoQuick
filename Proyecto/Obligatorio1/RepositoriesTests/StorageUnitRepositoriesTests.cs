using Model;
using Model.Enums;
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
        _storageUnit = new StorageUnit(0,AreaType.A, SizeType.Small, true,null );
    }
    
    
    [TestMethod]
    public void WhenAddingNewStorageUnitShouldAddItToRepository()
    {
        _storageUnitRepositories.AddToRepository(_storageUnit);
        StorageUnit storageUnitInRepo = _storageUnitRepositories.GetFromRepository(_storageUnit);
        Assert.AreEqual(_storageUnit.GetId(), storageUnitInRepo.GetId());
    }
    
}