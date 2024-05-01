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
        StorageUnit storageUnit= new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, storageUnitDto.Promotions);
        if (_storageUnitRepositories.GetFromRepository(storageUnitDto.Id) != null)
        {
            throw new LogicExceptions("Storage unit already exists");
        }
        else
        {
            _storageUnitRepositories.AddToRepository(storageUnit);
        }
    }
}