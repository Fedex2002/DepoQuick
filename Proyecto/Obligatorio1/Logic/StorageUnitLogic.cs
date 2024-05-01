using Model;
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
        List<Promotion> promotions = createListPromotions(storageUnitDto);
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

    public List<Promotion> createListPromotions(StorageUnitDto storageUnitDto)
    {
        return storageUnitDto.Promotions.Select(promotionDto => new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd)).ToList();
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
}