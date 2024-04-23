using RepositoriesInterface;
using Model;
using Model.Exceptions;

namespace Repositories;

public class StorageUnitRepositories : IRepositories<StorageUnit>
{
    private List<StorageUnit> _storageUnits = new List<StorageUnit>();
    
    public void AddToRepository(StorageUnit storageUnit)
    {
        if (ExistsInRepository(storageUnit.GetId()))
        {
            ThrowException();
        }
        _storageUnits.Add(storageUnit);
    }

    private static void ThrowException()
    {
        throw new RepositoryExceptions("The storage unit already exists");
    }

    public StorageUnit GetFromRepository(StorageUnit storageUnit)
    {
        return _storageUnits.Find(s => s.GetId() == storageUnit.GetId());
    }
    public bool ExistsInRepository(string id)
    {
        return _storageUnits.Any(s => s.GetId() == id);
    }
    public void RemoveFromRepository(StorageUnit storageUnit)
    {
        _storageUnits.Remove(storageUnit);
    }
}