using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Logic.DTOs;
using Logic.Interfaces;
using Model.Enums;
using Model.Exceptions;

namespace Logic;

public class StorageUnitController : IStorageUnitController, IDateRangeController
{
    private readonly StorageUnitsRepository _storageUnitRepositories;
    
    public StorageUnitController(ApplicationDbContext context)
    {
        _storageUnitRepositories = new StorageUnitsRepository(context);
    }
    
    public void CreateStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = CreateListPromotions(storageUnitDto);
        List<DateRange> availableDates = CreateListAvailableDates(storageUnitDto);
        StorageUnit storageUnit= new StorageUnit(storageUnitDto.Id, ConvertAreaTypeDtoToAreaType(storageUnitDto.Area), ConvertSizeTypeDtoToSizeType(storageUnitDto.Size), storageUnitDto.Climatization, promotions, availableDates);
        if (_storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id) != null)
        {
            IfStorageUnitAlreadyExistsThrowException();
        }
        else
        {
            _storageUnitRepositories.AddStorageUnit(storageUnit);
        }
    }

    private static void IfStorageUnitAlreadyExistsThrowException()
    {
        throw new LogicExceptions("Storage unit already exists");
    }

    public List<Promotion> CreateListPromotions(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = storageUnitDto.Promotions.Select(promotionDto => new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd)).ToList();
        return promotions;
    }
    
    public List<DateRange> CreateListAvailableDates(StorageUnitDto storageUnitDto)
    {
        List<DateRange> availableDates = storageUnitDto.AvailableDates.Select(dateRangeDto => new DateRange(dateRangeDto.StartDate, dateRangeDto.EndDate)).ToList();
        return availableDates;
    }

    public void RemoveStorageUnit(StorageUnitDto storageUnitDto)
    {
        StorageUnit storageUnitInRepo= _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id);
        if (_storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id) == null)
        {
            IfStorageUnitDoesNotExistThrowException();
        }
        else
        {
            _storageUnitRepositories.DeleteStorageUnit(storageUnitInRepo);
        }
    }

    private static void IfStorageUnitDoesNotExistThrowException()
    {
        throw new LogicExceptions("Storage unit does not exist");
    }

    public List<StorageUnitDto> GetStorageUnitsDto()
    {
        List<StorageUnitDto> storageUnitsDto = new List<StorageUnitDto>();
        foreach (var storageUnit in _storageUnitRepositories.GetAllStorageUnits())
        {
            StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.Id, ConvertAreaTypeToAreaTypeDto(storageUnit.Area), ConvertSizeTypeToSizeTypeDto(storageUnit.Size), storageUnit.Climatization, ChangeToPromotionsDto(storageUnit.Promotions), ChangeToDateRangeDto(storageUnit.AvailableDates));
            storageUnitsDto.Add(storageUnitDto);
        }
        return storageUnitsDto;
    }
    
    public List<PromotionDto> ChangeToPromotionsDto(List<Promotion> promotions)
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        if (promotions != null)
        {
            foreach(var promotion in promotions)
            {
                PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd);
                promotionsDto.Add(promotionDto);
            }
        }
        
        return promotionsDto;
    }
    
    public List<DateRangeDto> ChangeToDateRangeDto(List<DateRange> availableDates)
    {
        List<DateRangeDto> availableDatesDto = new List<DateRangeDto>();
        if (availableDates != null)
        {
            foreach (var dateRange in availableDates)
            {
                DateRangeDto dateRangeDto = new DateRangeDto(dateRange.StartDate, dateRange.EndDate);
                availableDatesDto.Add(dateRangeDto);
            }
        }
        return availableDatesDto;
    }
    
    public StorageUnitDto GetStorageUnitDtoFromId(string id)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(id);
        StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.Id, ConvertAreaTypeToAreaTypeDto(storageUnit.Area), ConvertSizeTypeToSizeTypeDto(storageUnit.Size), storageUnit.Climatization, ChangeToPromotionsDto(storageUnit.Promotions), ChangeToDateRangeDto(storageUnit.AvailableDates));
        return storageUnitDto;
    }
    
    public void DeletePromotionFromAllStorageUnits(PromotionDto promotionDto)
    {
        foreach (var storageUnit in _storageUnitRepositories.GetAllStorageUnits())
        {
            var promotions = storageUnit.Promotions.ToList();
            foreach (var promotion in promotions)
            {
                if (promotion.Label == promotionDto.Label)
                {
                    storageUnit.Promotions.Remove(promotion);
                }
            }
        }
    }
    
    public void AddAvailableDateRangeToStorageUnit(string id, DateRangeDto dateRangeDto)
    {
        IfDateRangeIsInvalidThrowException(dateRangeDto);
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(id);
        DateRange newDateRange = new DateRange(dateRangeDto.StartDate, dateRangeDto.EndDate);
        IfDateRangeAlreadyExistsThrowException(storageUnit, newDateRange);
        _storageUnitRepositories.AddAvailableDateToStorageUnit(id, newDateRange);
    }

    private static void IfDateRangeAlreadyExistsThrowException(StorageUnit storageUnit, DateRange newDateRange)
    {
        foreach (var dateRange in storageUnit.AvailableDates)
        {
            if (newDateRange.StartDate >= dateRange.StartDate && newDateRange.EndDate <= dateRange.EndDate)
            {
                DateRangeExistsSoThrowException();
            }
            if (newDateRange.StartDate <= dateRange.StartDate && newDateRange.EndDate >= dateRange.EndDate)
            {
                DateRangeExistsSoThrowException();
            }
            if (newDateRange.StartDate >= dateRange.StartDate && newDateRange.StartDate <= dateRange.EndDate)
            {
                DateRangeExistsSoThrowException();
            }
            if (newDateRange.EndDate >= dateRange.StartDate && newDateRange.EndDate <= dateRange.EndDate)
            {
                DateRangeExistsSoThrowException();
            }
        }
    }

    private static void DateRangeExistsSoThrowException()
    {
        throw new LogicExceptions("Date range already exists");
    }

    private static void IfDateRangeIsInvalidThrowException(DateRangeDto dateRangeDto)
    {
        if (dateRangeDto.EndDate < dateRangeDto.StartDate)
        {
            throw new LogicExceptions("Date error: end date is before start date");
        }
    }
    
    public List<StorageUnitDto> SearchAvailableStorageUnits(DateRangeDto dateRangeDto)
    {
        IfDateRangeIsInvalidThrowException(dateRangeDto);
        List<StorageUnitDto> availableStorageUnits = new List<StorageUnitDto>();
        foreach (var storageUnit in _storageUnitRepositories.GetAllStorageUnits())
        {
            foreach (var dateRange in storageUnit.AvailableDates)
            {
                if (dateRangeDto.StartDate >= dateRange.StartDate && dateRangeDto.EndDate <= dateRange.EndDate)
                {
                    StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.Id, ConvertAreaTypeToAreaTypeDto(storageUnit.Area), ConvertSizeTypeToSizeTypeDto(storageUnit.Size), storageUnit.Climatization, ChangeToPromotionsDto(storageUnit.Promotions), ChangeToDateRangeDto(storageUnit.AvailableDates));
                    availableStorageUnits.Add(storageUnitDto);
                } 
            }
        }
        IfThereIsNoStorageUnitWithThisDateRangeThrowException(availableStorageUnits);

        return availableStorageUnits;
    }

    private static void IfThereIsNoStorageUnitWithThisDateRangeThrowException(List<StorageUnitDto> availableStorageUnits)
    {
        if (availableStorageUnits.Count == 0)
        {
            throw new LogicExceptions("No storage units available for this date range");
        }
    }
    
    public void EliminateDateRangeFromStorageUnit(string id, DateRangeDto dateRangeDto)
    {
        if (dateRangeDto == null)
        {
            throw new LogicExceptions("Please select a date range to eliminate");
        }
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(id);
        foreach (var dateRange in storageUnit.AvailableDates.ToList())
        {
            if (dateRange.StartDate == dateRangeDto.StartDate && dateRange.EndDate == dateRangeDto.EndDate)
            {

                _storageUnitRepositories.DeleteAvailableDateFromStorageUnit(storageUnit.Id,dateRange);

            }
        }
    }

    public void ModifyOrRemoveDateRange(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id);
        foreach (var dateRange in storageUnit.AvailableDates.ToList())
        {
            if (dateRangeDto.StartDate == dateRange.StartDate && dateRangeDto.EndDate == dateRange.EndDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
            }
            if (dateRangeDto.StartDate == dateRange.StartDate && dateRangeDto.EndDate < dateRange.EndDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
                DateRange newDateRange = new DateRange(dateRangeDto.EndDate.AddDays(1), dateRange.EndDate);
                _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id).AvailableDates.Add(newDateRange);
            }
            if (dateRangeDto.EndDate == dateRange.EndDate && dateRangeDto.StartDate > dateRange.StartDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
                DateRange newDateRange = new DateRange(dateRange.StartDate, dateRangeDto.StartDate.AddDays(-1));
                _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id).AvailableDates.Add(newDateRange);
            }
            if (dateRangeDto.StartDate > dateRange.StartDate && dateRangeDto.EndDate < dateRange.EndDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
                DateRange newDateRange = new DateRange(dateRange.StartDate, dateRangeDto.StartDate.AddDays(-1));
                _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id).AvailableDates.Add(newDateRange);
                DateRange newDateRange2 = new DateRange(dateRangeDto.EndDate.AddDays(1), dateRange.EndDate);
                _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id).AvailableDates.Add(newDateRange2);
            }
        }
    }
    public void CheckIfDateStartAndDateEndAreIncludedInDateRange(DateTime dateStart, DateTime dateEnd, DateRangeDto dateRangeDto)
    {
        if (!(dateStart >= dateRangeDto.StartDate && dateEnd <= dateRangeDto.EndDate))
        {
            throw new LogicExceptions("Date range is not included in the available date range");
        }
    }
    
    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id);
        bool promotionIsInDateRange = storageUnit.Promotions.Any(promotion => dateRangeDto.StartDate >= promotion.DateStart && dateRangeDto.EndDate <= promotion.DateEnd);
        if (!promotionIsInDateRange)
        {
            storageUnit.Promotions = new List<Promotion>();
        }
        return storageUnit.CalculateStorageUnitPricePerDay();
    }

    public AreaTypeDto ConvertAreaTypeToAreaTypeDto(AreaType areaType)
    {
        return new AreaTypeDto(areaType);
    }
    
    public AreaType ConvertAreaTypeDtoToAreaType(AreaTypeDto areaTypeDto)
    {
        return (AreaType)areaTypeDto.Value;
    }
    
    public SizeTypeDto ConvertSizeTypeToSizeTypeDto(SizeType sizeType)
    {
        return new SizeTypeDto(sizeType);
    }
    
    public SizeType ConvertSizeTypeDtoToSizeType(SizeTypeDto sizeTypeDto)
    {
        return (SizeType)sizeTypeDto.Value;
    }
}