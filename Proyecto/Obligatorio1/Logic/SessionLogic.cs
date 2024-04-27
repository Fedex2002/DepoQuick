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
    
    public void Login(string email,string password)
    {
        User user = _userLogic.GetRepository().GetFromRepository(email);
        CurrentUser = _userLogic.Login(email,password); 
       
    }
    
    public void Logout(User user)
    {
        CurrentUser = null;
    }
}