using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Model;
using Logic.DTOs;
using Logic.Interfaces;
using Repositories;
using Model.Exceptions;

namespace Logic;

public class StorageUnitController : IStorageUnitController, IDateRangeController
{
    private readonly StorageUnitRepositories _storageUnitRepositories;
    
    public StorageUnitController(StorageUnitRepositories storageUnitRepositories)
    {
        _storageUnitRepositories = storageUnitRepositories;
    }
    
    public void CreateStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = CreateListPromotions(storageUnitDto);
        List<DateRange> availableDates = CreateListAvailableDates(storageUnitDto);
        StorageUnit storageUnit= new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions, availableDates);
        if (_storageUnitRepositories.GetFromRepository(storageUnitDto.Id) != null)
        {
            IfStorageUnitAlreadyExistsThrowException();
        }
        else
        {
            _storageUnitRepositories.AddToRepository(storageUnit);
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
        StorageUnit storageUnitInRepo= _storageUnitRepositories.GetFromRepository(storageUnitDto.Id);
        if (_storageUnitRepositories.GetFromRepository(storageUnitDto.Id) == null)
        {
            IfStorageUnitDoesNotExistThrowException();
        }
        else
        {
            _storageUnitRepositories.RemoveFromRepository(storageUnitInRepo);
        }
    }

    private static void IfStorageUnitDoesNotExistThrowException()
    {
        throw new LogicExceptions("Storage unit does not exist");
    }

    public List<StorageUnitDto> GetStorageUnitsDto()
    {
        List<StorageUnitDto> storageUnitsDto = new List<StorageUnitDto>();
        foreach (var storageUnit in _storageUnitRepositories.GetAllFromRepository())
        {
            StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.Id, storageUnit.Area, storageUnit.Size, storageUnit.Climatization, ChangeToPromotionsDto(storageUnit.Promotions), ChangeToDateRangeDto(storageUnit.AvailableDates));
            storageUnitsDto.Add(storageUnitDto);
        }
        return storageUnitsDto;
    }
    
    public List<PromotionDto> ChangeToPromotionsDto(List<Promotion> promotions)
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach(var promotion in promotions)
        {
            PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd);
            promotionsDto.Add(promotionDto);
        }

        return promotionsDto;
    }
    
    public List<DateRangeDto> ChangeToDateRangeDto(List<DateRange> availableDates)
    {
        List<DateRangeDto> availableDatesDto = new List<DateRangeDto>();
        foreach(var dateRange in availableDates)
        {
            DateRangeDto dateRangeDto = new DateRangeDto(dateRange.StartDate, dateRange.EndDate);
            availableDatesDto.Add(dateRangeDto);
        }

        return availableDatesDto;
    }
    
    public StorageUnitDto GetStorageUnitDtoFromId(string id)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetFromRepository(id);
        StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.Id, storageUnit.Area, storageUnit.Size, storageUnit.Climatization, ChangeToPromotionsDto(storageUnit.Promotions), ChangeToDateRangeDto(storageUnit.AvailableDates));
        return storageUnitDto;
    }
    
    public void DeletePromotionFromAllStorageUnits(PromotionDto promotionDto)
    {
        foreach (var storageUnit in _storageUnitRepositories.GetAllFromRepository())
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
        StorageUnit storageUnit = _storageUnitRepositories.GetFromRepository(id);
        DateRange newDateRange = new DateRange(dateRangeDto.StartDate, dateRangeDto.EndDate);
        IfDateRangeAlreadyExistsThrowException(storageUnit, newDateRange);
        storageUnit.AvailableDates.Add(newDateRange);
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
        foreach (var storageUnit in _storageUnitRepositories.GetAllFromRepository())
        {
            foreach (var dateRange in storageUnit.AvailableDates)
            {
                if (dateRangeDto.StartDate >= dateRange.StartDate && dateRangeDto.EndDate <= dateRange.EndDate)
                {
                    StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.Id, storageUnit.Area, storageUnit.Size, storageUnit.Climatization, ChangeToPromotionsDto(storageUnit.Promotions), ChangeToDateRangeDto(storageUnit.AvailableDates));
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
        StorageUnit storageUnit = _storageUnitRepositories.GetFromRepository(id);
        foreach (var dateRange in storageUnit.AvailableDates.ToList())
        {
            if (dateRange.StartDate == dateRangeDto.StartDate && dateRange.EndDate == dateRangeDto.EndDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
            }
        }
    }

    public void ModifyOrRemoveDateRange(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetFromRepository(storageUnitDto.Id);
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
                _storageUnitRepositories.GetFromRepository(storageUnitDto.Id).AvailableDates.Add(newDateRange);
            }
            if (dateRangeDto.EndDate == dateRange.EndDate && dateRangeDto.StartDate > dateRange.StartDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
                DateRange newDateRange = new DateRange(dateRange.StartDate, dateRangeDto.StartDate.AddDays(-1));
                _storageUnitRepositories.GetFromRepository(storageUnitDto.Id).AvailableDates.Add(newDateRange);
            }
            if (dateRangeDto.StartDate > dateRange.StartDate && dateRangeDto.EndDate < dateRange.EndDate)
            {
                storageUnit.AvailableDates.Remove(dateRange);
                DateRange newDateRange = new DateRange(dateRange.StartDate, dateRangeDto.StartDate.AddDays(-1));
                _storageUnitRepositories.GetFromRepository(storageUnitDto.Id).AvailableDates.Add(newDateRange);
                DateRange newDateRange2 = new DateRange(dateRangeDto.EndDate.AddDays(1), dateRange.EndDate);
                _storageUnitRepositories.GetFromRepository(storageUnitDto.Id).AvailableDates.Add(newDateRange2);
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
        StorageUnit storageUnit = _storageUnitRepositories.GetFromRepository(storageUnitDto.Id);
        bool promotionIsInDateRange = storageUnit.Promotions.Any(promotion => dateRangeDto.StartDate >= promotion.DateStart && dateRangeDto.EndDate <= promotion.DateEnd);
        if (!promotionIsInDateRange)
        {
            storageUnit.Promotions = new List<Promotion>();
        }
        return storageUnit.CalculateStorageUnitPricePerDay();
    }
}