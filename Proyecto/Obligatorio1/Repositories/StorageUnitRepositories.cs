using Logic;
using Model;

namespace Repositories;

public class StorageUnitRepositories : IRepositories<StorageUnit>
{
    private List<StorageUnit> _storageUnits = new List<StorageUnit>();
    
    public void AddToRepository(StorageUnit storageUnit)
    {
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