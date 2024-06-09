using Controllers.Dtos;

namespace Controllers.Interfaces;

public interface IPersonController
{
    public bool CheckIfPasswordIsCorrect(string password, string verifyPassword);
    public PersonDto Login(string email, string password);
    public void SignUp(PersonDto personDto);
    public PersonDto GetPersonDtoFromEmail(string personEmail);
    
    public bool CheckIfAdminExists();
}