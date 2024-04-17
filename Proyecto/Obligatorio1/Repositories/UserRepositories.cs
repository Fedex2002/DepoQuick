using Model;

namespace Repositories;
public class UserRepositories
{
    private List<User> _users = new List<User>();
    
    public void AddUser(User user)
    {
        return;
    }
    public User FindUser(User user)
    {
        User userInRepo = new User();
        return userInRepo;
    }
}