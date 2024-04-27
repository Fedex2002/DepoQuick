using Model;

namespace Logic;

public class SessionLogic
{
    private readonly PersonLogic _personLogic;
    public User CurrentUser { get; set; }
    
    public SessionLogic(PersonLogic personLogic)
    {
        _personLogic = personLogic;
    }
    
    public void Login(string email,string password)
    {
        CurrentUser = _personLogic.Login(email,password); 
    }
    
    public void Logout(Person person)
    {
        CurrentUser = null;
    }
}