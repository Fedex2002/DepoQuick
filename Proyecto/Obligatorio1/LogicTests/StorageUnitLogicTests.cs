using Model;
using Model.Enums;
using Repositories;
using Logic;
using Logic.DTOs;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class StorageUnitLogicTests
{
    private StorageUnitRepositories? _storageUnitRepo;
    private StorageUnitLogic? _storageUnitLogic;
    private StorageUnitDto? _storageUnitDto;
    private List<Promotion>? _promotions;
    private List<PromotionDto>? _promotionsDto;
    private Promotion? _promotion;
    private PromotionDto _promotionDto;
    [TestInitialize]
    public void TestInitialize()
    {
        _storageUnitRepo = new StorageUnitRepositories();
        _storageUnitLogic = new StorageUnitLogic(_storageUnitRepo);
        _promotions = new List<Promotion>();
        _promotionsDto = new List<PromotionDto>();
        _promotion = new Promotion("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionDto = new PromotionDto("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotions.Add(_promotion);
        _promotionsDto.Add(_promotionDto);
        _storageUnitDto = new StorageUnitDto("1", AreaType.B, SizeType.Medium, false, _promotionsDto);
    }
    [TestMethod]
    public void WhenCreatingPromotionListFromStorageUnitDtoShouldReturnPromotionList()
    {
        _promotions = _storageUnitLogic.CreateListPromotions(_storageUnitDto);
        Assert.IsNotNull(_promotions);
    }

    [TestMethod]
    public void WhenCreatingStorageUnitDtoEmptyShouldReturnEmptyStorageUnitDto()
    {
        StorageUnitDto storageUnitDto = new StorageUnitDto();
        Assert.IsNotNull(storageUnitDto);
    }
    
    [TestMethod]
    public void WhenStorageUnitIsCreatedShouldBeAddedToRepository()
    {
        _storageUnitLogic.CreateStorageUnit(_storageUnitDto);
        Assert.AreEqual(_storageUnitDto.Id, _storageUnitRepo.GetFromRepository(_storageUnitDto.Id).GetId());
        Assert.AreEqual(_storageUnitDto.Area, _storageUnitRepo.GetFromRepository(_storageUnitDto.Id).GetArea());
        Assert.AreEqual(_storageUnitDto.Size, _storageUnitRepo.GetFromRepository(_storageUnitDto.Id).GetSize());
        Assert.AreEqual(_storageUnitDto.Climatization, _storageUnitRepo.GetFromRepository(_storageUnitDto.Id).GetClimatization());

    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToCreateAnExistingStorageUnitShouldThrowException()
    {
        _storageUnitLogic.CreateStorageUnit(_storageUnitDto);
        _storageUnitLogic.CreateStorageUnit(_storageUnitDto);
    }
    
    [TestMethod]
    public void WhenStorageUnitIsEliminatedShouldBeRemovedFromRepository()
    {
        _storageUnitLogic.CreateStorageUnit(_storageUnitDto);
        _storageUnitLogic.RemoveStorageUnit(_storageUnitDto);
        Assert.IsNull(_storageUnitRepo.GetFromRepository(_storageUnitDto.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToRemoveANonExistingStorageUnitShouldThrowException()
    {
        _storageUnitLogic.RemoveStorageUnit(_storageUnitDto);
    }

    [TestMethod]
    public void WhenGettingStorageUnitsDtoShouldReturnAListOfStorageUnitsDto()
    {
        StorageUnit storageUnit = new StorageUnit("1", AreaType.B, SizeType.Medium, false, _promotions);
        _storageUnitRepo.AddToRepository(storageUnit);
        List<StorageUnitDto> storageUnitsDto = _storageUnitLogic.GetStorageUnitsDto();
        Assert.IsNotNull(storageUnitsDto);
    }

    [TestMethod]
    public void WhenGettingPromotionsFromRepositoryShouldChangeThemToPromotionsDto()
    {
        List<PromotionDto> promotionsDto = _storageUnitLogic.ChangeToPromotionsDto(_promotions);
        Assert.IsNotNull(promotionsDto);
    }

    [TestMethod]
    public void WhenGettingStorageUnitsDtoFromIdShouldReturnIt()
    {
        StorageUnit storageUnit = new StorageUnit("1", AreaType.B, SizeType.Medium, false, _promotions);
        _storageUnitRepo.AddToRepository(storageUnit);
        StorageUnitDto storageUnitDto = _storageUnitLogic.GetStorageUnitDtoFromId(storageUnit.GetId());
        Assert.AreEqual(storageUnit.GetId(),storageUnitDto.Id);
    }

    [TestMethod]
    public void WhenPromotionIsDeletedShouldDeleteItFromAllStorageUnits()
    {
        _storageUnitLogic.CreateStorageUnit(_storageUnitDto);
        _storageUnitLogic.DeletePromotionFromStorageUnit(_promotionDto);
    }
}