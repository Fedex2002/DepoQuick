
namespace Logic.DTOs;

public class PersonDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public string Surname { get; set; }
    
    public string Password { get; set; }

    public PersonDto()
    {
    }


  

    public PersonDto(string name, string surname, string email, string password)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
    }
    
}