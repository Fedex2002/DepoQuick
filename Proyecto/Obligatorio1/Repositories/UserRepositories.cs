using RepositoriesInterface;
using Model;
using Model.Exceptions;

namespace Repositories;
public class UserRepositories : IRepositories<User>
{
    private List<User> _users = new List<User>();
    
     public void AddToRepository(User user)
     {
         if (ExistsInRepository(user.GetEmail()))
         {
             ThrowException();
         }
         _users.Add(user);
     }

    private static void ThrowException()
    {
        throw new RepositoryExceptions("The user already exists");
    }

    public User GetFromRepository(User user)
    {
        User userInRepo = _users.Find(u => u.GetEmail() == user.GetEmail());
        return userInRepo;
    }

    public bool ExistsInRepository(string email)
    {
        return _users.Any(u => u.GetEmail() == email);
    }
    public void RemoveFromRepository(User user)
    {
        _users.Remove(user);
    }
  
}