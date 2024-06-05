using Logic.DTOs;
using Model;

namespace Logic;

public class SessionLogic
{
    private readonly PersonController _personController;
    public PersonDto CurrentPerson { get; set; }
    
    public SessionLogic(PersonController personController)
    {
        _personController = personController;
    }
    
    public void Login(string email,string password)
    {
        CurrentPerson = _personController.Login(email,password); 
    }
    
    public void Logout()
    {
        CurrentPerson = null;
    }
}