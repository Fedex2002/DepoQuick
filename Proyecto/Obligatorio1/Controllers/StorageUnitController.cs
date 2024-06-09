
using Controllers.Dtos;
using Controllers.Interfaces;
using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Enums;

using Model.Exceptions;

namespace Controllers;

public class StorageUnitController : IStorageUnitController, IDateRangeController
{
    private readonly StorageUnitsRepository _storageUnitRepositories;



    private readonly PromotionsRepository _promotionsRepository;


    public StorageUnitController(ApplicationDbContext context)
    {
        _storageUnitRepositories = new StorageUnitsRepository(context);
        _promotionsRepository = new PromotionsRepository(context);

    }
    
    public void CreateStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = CreateListPromotions(storageUnitDto);
        List<DateRange> availableDates = CreateListAvailableDates(storageUnitDto);

        StorageUnit storageUnit= new StorageUnit(storageUnitDto.Id, ConvertAreaTypeDtoToAreaType(storageUnitDto.Area), ConvertSizeTypeDtoToSizeType(storageUnitDto.Size), storageUnitDto.Climatization, promotions, availableDates);

        _storageUnitRepositories.AddStorageUnit(storageUnit);
        
    }
    
    public List<Promotion> CreateListPromotions(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = new List<Promotion>();
        foreach (var promotion in _promotionsRepository.GetAllPromotions())
        {
            foreach (var promotionDto in storageUnitDto.Promotions)
            {
                if (promotion.Label == promotionDto.Label)
                {
                    promotions.Add(promotion);
                }
            }
        }
        return promotions;
    }

    private List<DateRange> CreateListAvailableDates(StorageUnitDto storageUnitDto)
    {
        List<DateRange> availableDates = storageUnitDto.AvailableDates.Select(dateRangeDto => new DateRange(dateRangeDto.StartDate, dateRangeDto.EndDate)).ToList();
        return availableDates;
    }

    public void RemoveStorageUnit(StorageUnitDto storageUnitDto)
    {
        StorageUnit storageUnitInRepo= _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id);
        _storageUnitRepositories.DeleteStorageUnit(storageUnitInRepo);

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
    
    private List<DateRangeDto> ChangeToDateRangeDto(List<DateRange> availableDates)
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
            var isContained = newDateRange.StartDate >= dateRange.StartDate && newDateRange.EndDate <= dateRange.EndDate;
            var contains = newDateRange.StartDate <= dateRange.StartDate && newDateRange.EndDate >= dateRange.EndDate;
            var startsWithinRange = newDateRange.StartDate >= dateRange.StartDate && newDateRange.StartDate <= dateRange.EndDate;
            var endsWithinRange = newDateRange.EndDate >= dateRange.StartDate && newDateRange.EndDate <= dateRange.EndDate;
            if (isContained || contains || startsWithinRange || endsWithinRange)
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
        DateRangeExists(dateRangeDto);
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(id);
        foreach (var dateRange in storageUnit.AvailableDates.ToList())
        {
            if (dateRange.StartDate == dateRangeDto.StartDate && dateRange.EndDate == dateRangeDto.EndDate)
            {

                _storageUnitRepositories.DeleteAvailableDateFromStorageUnit(storageUnit.Id,dateRange);
            }
        }
    }

    private static void DateRangeExists(DateRangeDto dateRangeDto)
    {
        if (dateRangeDto == null)
        {
            throw new LogicExceptions("Please select a date range to eliminate");
        }
    }

    public void ModifyOrRemoveDateRange(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetStorageUnitFromId(storageUnitDto.Id);
        foreach (var dateRange in storageUnit.AvailableDates.ToList())
        {
            if (dateRangeDto.StartDate == dateRange.StartDate && dateRangeDto.EndDate == dateRange.EndDate)
            {
                _storageUnitRepositories.DeleteAvailableDateFromStorageUnit(storageUnit.Id, dateRange);
            }
            else if (dateRangeDto.StartDate == dateRange.StartDate && dateRangeDto.EndDate < dateRange.EndDate)
            {

                UpdateDateRanges(storageUnit.Id, dateRange, new DateRange(dateRangeDto.EndDate.AddDays(1), dateRange.EndDate));

            }
            else if (dateRangeDto.EndDate == dateRange.EndDate && dateRangeDto.StartDate > dateRange.StartDate)
            {

                UpdateDateRanges(storageUnit.Id, dateRange, new DateRange(dateRange.StartDate, dateRangeDto.StartDate.AddDays(-1)));

            }
            else if (dateRangeDto.StartDate > dateRange.StartDate && dateRangeDto.EndDate < dateRange.EndDate)
            {

                UpdateDateRanges(storageUnit.Id, dateRange, new DateRange(dateRange.StartDate, dateRangeDto.StartDate.AddDays(-1)));
                UpdateDateRanges(storageUnit.Id, dateRange, new DateRange(dateRangeDto.EndDate.AddDays(1), dateRange.EndDate));

            }
        }
    }
    
    private void UpdateDateRanges(string storageUnitId, DateRange oldDateRange, DateRange newDateRange)
    {
        _storageUnitRepositories.DeleteAvailableDateFromStorageUnit(storageUnitId, oldDateRange);
        _storageUnitRepositories.AddAvailableDateToStorageUnit(storageUnitId, newDateRange);
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
        var promotionsInDateRange = _promotionsRepository.GetAllPromotions()
            .Where(promotion => promotion.DateStart <= dateRangeDto.EndDate && promotion.DateEnd >= dateRangeDto.StartDate)
            .ToList();
        storageUnit.Promotions = promotionsInDateRange.Any() ? promotionsInDateRange : new List<Promotion>();

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