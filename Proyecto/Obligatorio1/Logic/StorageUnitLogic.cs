using Model;
using Model.Enums;
using Logic.DTOs;
using Repositories;
using Model.Exceptions;

namespace Logic;

public class StorageUnitLogic
{
    private StorageUnitRepositories _storageUnitRepositories;
    
    public StorageUnitLogic(StorageUnitRepositories storageUnitRepositories)
    {
        _storageUnitRepositories = storageUnitRepositories;
    }
    
    public void CreateStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = CreateListPromotions(storageUnitDto);
        StorageUnit storageUnit= new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions);
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

    public void RemoveStorageUnit(StorageUnitDto storageUnitDto)
    {
        StorageUnit storageUnitInRepo= _storageUnitRepositories.GetFromRepository(storageUnitDto.Id);
        if (_storageUnitRepositories.GetFromRepository(storageUnitDto.Id) == null)
        {
            throw new LogicExceptions("Storage unit does not exist");
        }
        else
        {
            _storageUnitRepositories.RemoveFromRepository(storageUnitInRepo);
        }
    }
    
    public List<StorageUnitDto> GetStorageUnitsDto()
    {
        List<StorageUnitDto> storageUnitsDto = new List<StorageUnitDto>();
        foreach (var storageUnit in _storageUnitRepositories.GetAllFromRepository())
        {
            StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.GetId(), storageUnit.GetArea(), storageUnit.GetSize(), storageUnit.GetClimatization(), ChangeToPromotionsDto(storageUnit.GetPromotions()));
            storageUnitsDto.Add(storageUnitDto);
        }
        return storageUnitsDto;
    }
    
    public List<PromotionDto> ChangeToPromotionsDto(List<Promotion> promotions)
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach(var promotion in promotions)
        {
            PromotionDto promotionDto = new PromotionDto(promotion.GetLabel(), promotion.GetDiscount(), promotion.GetDateStart(), promotion.GetDateEnd());
            promotionsDto.Add(promotionDto);
        }

        return promotionsDto;
    }
    
    public StorageUnitDto GetStorageUnitDtoFromId(string id)
    {
        StorageUnit storageUnit = _storageUnitRepositories.GetFromRepository(id);
        StorageUnitDto storageUnitDto = new StorageUnitDto(storageUnit.GetId(), storageUnit.GetArea(), storageUnit.GetSize(), storageUnit.GetClimatization(), ChangeToPromotionsDto(storageUnit.GetPromotions()));
        return storageUnitDto;
    }
}