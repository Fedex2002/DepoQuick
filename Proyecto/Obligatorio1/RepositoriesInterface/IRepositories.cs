namespace RepositoriesInterface;

public interface IRepositories<T>
{
    public void AddToRepository(T item);
    public void RemoveFromRepository(T item);
    public bool ExistsInRepository(string item);
    public T GetFromRepository(string item);
    public List<T> GetAllFromRepository();
    
}