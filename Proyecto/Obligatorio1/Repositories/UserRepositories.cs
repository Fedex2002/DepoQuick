using Model;

namespace Repositories;
public class UserRepositories
{
    private List<User> _users = new List<User>();
    
    public void AddUser(User user)
    {
        _users.Add(user);
    }
    public User FindUser(User user)
    {
        User userInRepo = _users.Find(u => u.GetEmail() == user.GetEmail());
        return userInRepo;
    }
}