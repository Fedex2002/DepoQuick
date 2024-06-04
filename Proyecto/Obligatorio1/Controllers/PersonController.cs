using Logic;
using Logic.DTOs;

namespace Controllers;

public class PersonController
{
    private PersonLogic _personLogic;
    public PersonController(PersonLogic personLogic)
    {
        _personLogic = personLogic;
    }
   
    public PersonDto Login(string email, string password)
    {
        return _personLogic.Login(email, password);
    }
}