using Model;
using Model.Exceptions;

namespace Repositories;
public class UserRepositories
{
    private List<User> _users = new List<User>();
    
    public void AddUser(User user)
    {
        if (UserExists(user))
        {
            throw new RepositoryExceptions("The user already exists");
        }
        _users.Add(user);
    }
    public User FindUser(User user)
    {
        User userInRepo = _users.Find(u => u.GetEmail() == user.GetEmail());
        return userInRepo;
    }

    public bool UserExists(User user)
    {
        return _users.Any(u => u.GetEmail() == user.GetEmail());
    }
}