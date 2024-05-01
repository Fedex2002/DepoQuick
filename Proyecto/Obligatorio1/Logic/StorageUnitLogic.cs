using Model;
using Logic.DTOs;
using Repositories;

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
        _storageUnitRepositories.AddToRepository(storageUnit);
    }
}