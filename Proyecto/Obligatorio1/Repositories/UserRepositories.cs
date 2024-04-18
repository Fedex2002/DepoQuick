using Logic;
using Model;
using Model.Exceptions;

namespace Repositories;
public class UserRepositories : IRepositories<User>
{
    private List<User> _users = new List<User>();
    
     public void AddToRepository(User user)
     {
         if (ExistsInRepository(user))
         {
             ThrowException();
         }
         _users.Add(user);
     }

    private static void ThrowException()
    {
        throw new RepositoryExceptions("The user already exists");
    }

    public User FindUser(User user)
    {
        User userInRepo = _users.Find(u => u.GetEmail() == user.GetEmail());
        return userInRepo;
    }

    public bool ExistsInRepository(User user)
    {
        return _users.Any(u => u.GetEmail() == user.GetEmail());
    }
    public void RemoveFromRepository(User user)
    {
        _users.Remove(user);
    }
  
}