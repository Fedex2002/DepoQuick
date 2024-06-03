using DataAccess.Context;
using Model;
using Model.Exceptions;

namespace DataAccess.Repository;

public class StorageUnitsRepository
{
    private ApplicationDbContext _database;

    public StorageUnitsRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public void AddStorageUnit(StorageUnit storageUnit)
    {
        if (StorageUnitAlreadyExists(storageUnit))
        {
            StorageUnitAlreadyExistsSoThrowException();
        }
        
        _database.StorageUnits.Add(storageUnit);
        
        _database.SaveChanges();
    }

    private static void StorageUnitAlreadyExistsSoThrowException()
    {
        throw new RepositoryExceptions("Storage unit already exists");
    }

    public bool StorageUnitAlreadyExists(StorageUnit storageUnit)
    {
        return _database.StorageUnits.Any(s => s == storageUnit);
    }
}