using Model;
using Repositories;
using Logic;
using Logic.DTOs;

namespace LogicTests;

[TestClass]
public class StorageUnitLogicTests
{
    private StorageUnitRepositories _storageUnitRepo;
    private StorageUnitLogic _storageUnitLogic;
    private StorageUnitDto _storageUnitDto;
    private List<Promotion> _promotions;
    private Promotion _promotion;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _storageUnitRepo = new StorageUnitRepositories();
        _storageUnitLogic = new StorageUnitLogic(_storageunitrepo);
        _promotions = new List<Promotion>();
        _promotion = new Promotion("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotions.Add(_promotion);
    }

    [TestMethod]
    public void WhenCreatingStorageUnitDtoEmptyShouldReturnEmptyStorageUnitDto()
    {
        StorageUnitDto storageUnitDto = new StorageUnitDto();
        Assert.IsNotNull(storageUnitDto);
    }
}