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

        AddNewStorageUnitToStorageUnitsTable(storageUnit);
    }

    private void AddNewStorageUnitToStorageUnitsTable(StorageUnit storageUnit)
    {
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
    
    public void DeleteStorageUnit(StorageUnit storageUnit)
    {
        StorageUnit dbStorageUnit = GetStorageUnit(storageUnit.Id);
        if (dbStorageUnit != null)
        {
            _database.StorageUnits.Remove(dbStorageUnit);
            _database.SaveChanges();
        }
    }
    
    public StorageUnit GetStorageUnit(string id)
    {
        return _database.StorageUnits.FirstOrDefault(s => s.Id == id);
    }
    
    public List<StorageUnit> GetAllStorageUnits()
    {
        return _database.StorageUnits.ToList();
    }
}