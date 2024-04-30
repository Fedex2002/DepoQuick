namespace Logic.DTOs;

public class AdministratorDto : PersonDto
{
    public AdministratorDto()
    {
    }


    public AdministratorDto(string name, string surname, string email, string password) : base(name, surname, email, password)
    {
    }
    
}