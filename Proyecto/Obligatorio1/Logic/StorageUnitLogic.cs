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
            throw new LogicExceptions("Storage unit already exists");
        }
        else
        {
            _storageUnitRepositories.AddToRepository(storageUnit);
        }
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
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        StorageUnitDto storageUnitDto = new StorageUnitDto("1", AreaType.B, SizeType.Medium, false, promotionsDto);
        storageUnitsDto.Add(storageUnitDto);
        return storageUnitsDto;
    }
}