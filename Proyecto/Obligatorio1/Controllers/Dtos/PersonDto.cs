
namespace Controllers.Dtos;

public class PersonDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    
    public bool IsAdmin { get; set; }

    public PersonDto()
    {
    }
    
    public PersonDto(string name, string surname, string email, string password,bool isAdmin)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }
    
}