using Logic.DTOs;
using Model;

namespace Logic;

public class SessionLogic
{
    private readonly PersonLogic _personLogic;
    public PersonDto CurrentPerson { get; set; }
    
    public SessionLogic(PersonLogic personLogic)
    {
        _personLogic = personLogic;
    }
    
    public void Login(string email,string password)
    {
        CurrentPerson = _personLogic.Login(email,password); 
    }
    
    public void Logout()
    {
        CurrentPerson = null;
    }
}