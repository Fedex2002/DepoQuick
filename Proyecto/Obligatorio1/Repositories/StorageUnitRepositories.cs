using Model;

namespace Repositories;

public class StorageUnitRepositories
{
    private List<StorageUnit> _storageUnits = new List<StorageUnit>();
    
    public void AddToRepository(StorageUnit storageUnit)
    {
        _storageUnits.Add(storageUnit);
    }
    public StorageUnit GetFromRepository(StorageUnit storageUnit)
    {
        return storageUnit;
    }
}