using Model;

namespace Logic;

public class SessionLogic
{
    private readonly UserLogic _userLogic;
    public User CurrentUser { get; set; }
    
    public SessionLogic(UserLogic userLogic)
    {
        _userLogic = userLogic;
    }
    
    public void Login(User user)
    {
        CurrentUser = _userLogic.Login(user.GetEmail(), user.GetPassword());
    }
    
    public void Logout(User user)
    {
        CurrentUser = null;
    }
}