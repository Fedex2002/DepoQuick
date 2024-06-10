using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
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
      
        if (!StorageUnitAlreadyExists(storageUnit))
        {
            throw new RepositoryExceptions("Storage unit does not exist");
        }
        StorageUnit dbStorageUnit = GetStorageUnitFromId(storageUnit.Id);
        var bookingsToDelete = _database.Bookings
            .Where(b => b.StorageUnit.Id == storageUnit.Id)
            .ToList();

        _database.Bookings.RemoveRange(bookingsToDelete);
        _database.StorageUnits.Remove(dbStorageUnit);
        _database.SaveChanges();
     

    
   
    }
    
    public StorageUnit GetStorageUnitFromId(string id)
    {
        return _database.StorageUnits
            .Include(s => s.Promotions)
            .Include(s => s.AvailableDates)
            .FirstOrDefault(s => s.Id == id);
    }
    public List<StorageUnit> GetAllStorageUnits()
    {
       return _database.StorageUnits
                   .Include(s => s.AvailableDates)
                   .Include(s => s.Promotions)
                   .ToList();
    }

    public void AddAvailableDateToStorageUnit(string storageUnitId, DateRange dateRange)
    {
        StorageUnit storageUnit = GetStorageUnitFromId(storageUnitId);
        storageUnit.AvailableDates.Add(dateRange);
        _database.SaveChanges();
    }

    
    public void DeleteAvailableDateFromStorageUnit(string storageUnitId, DateRange dateRange)
    {
        StorageUnit storageUnit = GetStorageUnitFromId(storageUnitId);
        storageUnit.AvailableDates.Remove(dateRange);
        _database.SaveChanges();
    }

}