using Logic;
using Model;
using Model.Exceptions;

namespace Repositories;

public class StorageUnitRepositories : IRepositories<StorageUnit>
{
    private List<StorageUnit> _storageUnits = new List<StorageUnit>();
    
    public void AddToRepository(StorageUnit storageUnit)
    {
        if (ExistsInRepository(storageUnit))
        {
            throw new RepositoryExceptions("The storage unit already exists");
        }
        _storageUnits.Add(storageUnit);
    }
    public StorageUnit GetFromRepository(StorageUnit storageUnit)
    {
        return _storageUnits.Find(s => s.GetId() == storageUnit.GetId());
    }
    public bool ExistsInRepository(StorageUnit storageUnit)
    {
        return _storageUnits.Any(s => s.GetId() == storageUnit.GetId());
    }
    public void RemoveFromRepository(StorageUnit storageUnit)
    {
        _storageUnits.Remove(storageUnit);
    }
}