using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Enums;
using Repositories;
using Logic;
using Logic.DTOs;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class StorageUnitControllerTests
{
    private ApplicationDbContext _context;
    private StorageUnitController _storageUnitController;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private StorageUnitsRepository _storageUnitsRepo;
    private StorageUnitDto _storageUnitDto;
    private List<Promotion> _promotions;
    private List<PromotionDto> _promotionsDto;
    private Promotion _promotion;
    private PromotionDto _promotionDto;
    private List<DateRange> _availableDates;
    private List<DateRangeDto> _availableDatesDto;
    private DateRange _dateRange;
    private DateRangeDto _dateRangeDto;

    private AreaTypeDto _areaTypeDto;
    private SizeTypeDto _sizeTypeDto;

    
    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _storageUnitsRepo = new StorageUnitsRepository(_context);
        _storageUnitController = new StorageUnitController(_context);
        _promotions = new List<Promotion>();
        _promotionsDto = new List<PromotionDto>();
        _promotion = new Promotion("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionDto = new PromotionDto("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotions.Add(_promotion);
        _promotionsDto.Add(_promotionDto);
        _availableDates = new List<DateRange>();
        _dateRange = new DateRange(new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _availableDates.Add(_dateRange);
        _availableDatesDto = new List<DateRangeDto>();
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _availableDatesDto.Add(_dateRangeDto);

        _areaTypeDto = new AreaTypeDto(AreaType.A);
        _sizeTypeDto = new SizeTypeDto(SizeType.Medium);
        _storageUnitDto = new StorageUnitDto("1", _areaTypeDto, _sizeTypeDto, false, _promotionsDto, _availableDatesDto);

    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void WhenCreatingPromotionListFromStorageUnitDtoShouldReturnPromotionList()
    {
        _promotions = _storageUnitController.CreateListPromotions(_storageUnitDto);
        Assert.IsNotNull(_promotions);
    }

    [TestMethod]
    public void WhenCreatingStorageUnitDtoEmptyShouldReturnEmptyStorageUnitDto()
    {
        StorageUnitDto storageUnitDto = new StorageUnitDto();
        Assert.IsNotNull(storageUnitDto);
    }
    
    [TestMethod]
    public void WhenCreatingDateRangeDtoEmptyShouldReturnEmptyDateRangeDto()
    {
        DateRangeDto dateRangeDto = new DateRangeDto();
        Assert.IsNotNull(dateRangeDto);
    }
    
    [TestMethod]
    public void WhenStorageUnitIsCreatedShouldBeAddedToRepository()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        Assert.AreEqual(_storageUnitDto.Id, _storageUnitController.GetStorageUnitDtoFromId(_storageUnitDto.Id).Id);
        Assert.AreEqual(_storageUnitDto.Area, _storageUnitController.GetStorageUnitDtoFromId(_storageUnitDto.Id).Area);
        Assert.AreEqual(_storageUnitDto.Size, _storageUnitController.GetStorageUnitDtoFromId(_storageUnitDto.Id).Size);
        Assert.AreEqual(_storageUnitDto.Climatization, _storageUnitController.GetStorageUnitDtoFromId(_storageUnitDto.Id).Climatization);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToCreateAnExistingStorageUnitShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
    }
    
    [TestMethod]
    public void WhenStorageUnitIsEliminatedShouldBeRemovedFromRepository()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.RemoveStorageUnit(_storageUnitDto);
        Assert.IsNull(_storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id));
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToRemoveANonExistingStorageUnitShouldThrowException()
    {
        _storageUnitController.RemoveStorageUnit(_storageUnitDto);
    }

    [TestMethod]
    public void WhenGettingStorageUnitsDtoShouldReturnAListOfStorageUnitsDto()
    {
        StorageUnit storageUnit = new StorageUnit("1", AreaType.B, SizeType.Medium, false, _promotions, _availableDates);
        _storageUnitsRepo.AddStorageUnit(storageUnit);
        List<StorageUnitDto> storageUnitsDto = _storageUnitController.GetStorageUnitsDto();
        Assert.IsNotNull(storageUnitsDto);
    }

    [TestMethod]
    public void WhenGettingPromotionsFromRepositoryShouldChangeThemToPromotionsDto()
    {
        List<PromotionDto> promotionsDto = _storageUnitController.ChangeToPromotionsDto(_promotions);
        Assert.IsNotNull(promotionsDto);
    }

    [TestMethod]
    public void WhenGettingStorageUnitsDtoFromIdShouldReturnIt()
    {
        StorageUnit storageUnit = new StorageUnit("1", AreaType.B, SizeType.Medium, false, _promotions, _availableDates);
        _storageUnitsRepo.AddStorageUnit(storageUnit);
        StorageUnitDto storageUnitDto = _storageUnitController.GetStorageUnitDtoFromId(storageUnit.Id);
        Assert.AreEqual(storageUnit.Id,storageUnitDto.Id);
    }

    [TestMethod]
    public void WhenPromotionIsDeletedShouldDeleteItFromAllStorageUnits()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.DeletePromotionFromAllStorageUnits(_promotionDto);
    }

    [TestMethod]
    public void WhenAvailableDateRangeIsAddedToAStorageUnitShouldSetIt()
    {

        _storageUnitDto = new StorageUnitDto("5", _areaTypeDto, _sizeTypeDto, true, _promotionsDto, new List<DateRangeDto>());

        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.AddAvailableDateRangeToStorageUnit(_storageUnitDto.Id, _dateRangeDto);
        Assert.AreEqual(_dateRangeDto.StartDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].StartDate);
        Assert.AreEqual(_dateRangeDto.EndDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].EndDate);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToAddAnIncorrectAvailableDateRangeToAStorageUnitShouldThrowException()
    {
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 10, 15), new DateTime(2024, 5, 15));
        _storageUnitController.AddAvailableDateRangeToStorageUnit(_storageUnitDto.Id, _dateRangeDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToAddASameOrIncludedExistingAvailableDateRangeShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.AddAvailableDateRangeToStorageUnit(_storageUnitDto.Id, _dateRangeDto);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToAddAnAvailableDateRangeThatCoversAnExistingOneShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 1), new DateTime(2024, 10, 30));
        _storageUnitController.AddAvailableDateRangeToStorageUnit(_storageUnitDto.Id, _dateRangeDto);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToAddAnAvailableDateRangeThatStartDateExistsInACreatedRangeShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 10, 15), new DateTime(2024, 10, 30));
        _storageUnitController.AddAvailableDateRangeToStorageUnit(_storageUnitDto.Id, _dateRangeDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToAddAnAvailableDateRangeThatEndDateExistsInACreatedRangeShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 10), new DateTime(2024, 7, 15));
        _storageUnitController.AddAvailableDateRangeToStorageUnit(_storageUnitDto.Id, _dateRangeDto);
    }

    [TestMethod]
    public void WhenSearchingStorageUnitsWithDateRangeShouldReturnThem()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 27), new DateTime(2024, 10, 12));
        List<StorageUnitDto> storageUnitsDto = _storageUnitController.SearchAvailableStorageUnits(_dateRangeDto);
        Assert.AreEqual(1 ,storageUnitsDto.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToSearchStorageUnitsWithDateRangeThatIsInvalidShouldThrowException()
    {
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 10, 15), new DateTime(2024, 5, 15));
        _storageUnitController.SearchAvailableStorageUnits(_dateRangeDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenSearchingStorageUnitsWithDateRangeThatDoesNotExistsShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 10, 15), new DateTime(2024, 10, 30));
        _storageUnitController.SearchAvailableStorageUnits(_dateRangeDto);
    }
    
    [TestMethod]
    public void WhenSelectingDateRangeShouldEliminateItFromStorageUnit()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.EliminateDateRangeFromStorageUnit(_storageUnitDto.Id, _dateRangeDto);
        Assert.AreEqual(0, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenNotSelectingADateRangeToRemoveFromStorageUnitShouldThrowException()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = null;
        _storageUnitController.EliminateDateRangeFromStorageUnit(_storageUnitDto.Id, _dateRangeDto);
    }

    [TestMethod]
    public void WhenUserMakesABookingInADateRangeShouldReduceDateRangeOfStorageUnitOrRemoveIt()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _storageUnitController.ModifyOrRemoveDateRange(_storageUnitDto, _dateRangeDto);
        Assert.AreEqual(0, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates.Count);
        
        _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates.Add(_dateRange);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 15), new DateTime(2024, 8, 15));
        _storageUnitController.ModifyOrRemoveDateRange(_storageUnitDto, _dateRangeDto);
        DateRange newRange = new DateRange(new DateTime(2024, 8, 16), new DateTime(2024, 10, 15));
        Assert.AreEqual(newRange.StartDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].StartDate);
        Assert.AreEqual(newRange.EndDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].EndDate);
        
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 9, 15), new DateTime(2024, 10, 15));
        _storageUnitController.ModifyOrRemoveDateRange(_storageUnitDto, _dateRangeDto);
        newRange = new DateRange(new DateTime(2024, 8, 16), new DateTime(2024, 9, 14));
        Assert.AreEqual(newRange.StartDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].StartDate);
        Assert.AreEqual(newRange.EndDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].EndDate);
        
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 8, 26), new DateTime(2024, 9, 6));
        _storageUnitController.ModifyOrRemoveDateRange(_storageUnitDto, _dateRangeDto);
        newRange = new DateRange(new DateTime(2024, 8, 16), new DateTime(2024, 8, 25));
        Assert.AreEqual(newRange.StartDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].StartDate);
        Assert.AreEqual(newRange.EndDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[0].EndDate);
        newRange = new DateRange(new DateTime(2024, 9, 7), new DateTime(2024, 9, 14));
        Assert.AreEqual(newRange.StartDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[1].StartDate);
        Assert.AreEqual(newRange.EndDate, _storageUnitsRepo.GetStorageUnitFromId(_storageUnitDto.Id).AvailableDates[1].EndDate);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void IfSelectedStartDateAndEndDateOfBookingIsNotInDateRangeShouldThrowException()
    {
        DateTime startDate = new DateTime(2024, 10, 15);
        DateTime endDate = new DateTime(2024, 10, 30);
        _storageUnitController.CheckIfDateStartAndDateEndAreIncludedInDateRange(startDate, endDate, _dateRangeDto);
    }
    
    [TestMethod]
    public void WhenUserEntersPageBookingsShouldShowPricePerDayOfStorageUnit()
    {
        _storageUnitController.CreateStorageUnit(_storageUnitDto);
        _dateRangeDto = new DateRangeDto(new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        Assert.AreEqual(56.25, _storageUnitController.CalculateStorageUnitPricePerDay(_storageUnitDto, _dateRangeDto));
    }


    [TestMethod]
    public void WhenConvertingAreaTypeToAreaTypeDtoShouldReturnIt()
    {
        AreaType areaType = AreaType.A;
        AreaTypeDto areaTypeDto = _storageUnitController.ConvertAreaTypeToAreaTypeDto(areaType);
        Assert.AreEqual((int)areaType, areaTypeDto.Value);
        Assert.AreEqual(areaType.ToString(), areaTypeDto.Name);
    }
    
    [TestMethod]
    public void WhenConvertingAreaTypeDtoToAreaTypeShouldReturnIt()
    {
        AreaTypeDto areaTypeDto = new AreaTypeDto(AreaType.A);
        AreaType areaType = _storageUnitController.ConvertAreaTypeDtoToAreaType(areaTypeDto);
        Assert.AreEqual((int)areaType, areaTypeDto.Value);
        Assert.AreEqual(areaType.ToString(), areaTypeDto.Name);
    }

    [TestMethod]
    public void WhenConvertingSizeTypeToSizeTypeDtoShouldReturnIt()
    {
        SizeType sizeType = SizeType.Small;
        SizeTypeDto sizeTypeDto = _storageUnitController.ConvertSizeTypeToSizeTypeDto(sizeType);
        Assert.AreEqual((int)sizeType, sizeTypeDto.Value);
        Assert.AreEqual(sizeType.ToString(), sizeTypeDto.Name);
    }
    
    [TestMethod]
    public void WhenConvertingSizeTypeDtoToSizeTypeShouldReturnIt()
    {
        SizeTypeDto sizeTypeDto = new SizeTypeDto(SizeType.Small);
        SizeType sizeType = _storageUnitController.ConvertSizeTypeDtoToSizeType(sizeTypeDto);
        Assert.AreEqual((int)sizeType, sizeTypeDto.Value);
        Assert.AreEqual(sizeType.ToString(), sizeTypeDto.Name);
    }

}