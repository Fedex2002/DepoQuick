namespace Model;

public class Person
{
    private string _name { get; set; }
    private string _surname { get; set; }
    private string _email { get; set; }
    private string _password { get; set; }

    public Person()
    {
        
    }
    public Person(string name, string surname, string email, string password)
    {
        _name = name;
        _surname = surname;
        _email = email;
        _password = password;
    }
    
    public bool ValidatePassword()
    {
        return true;
    }

    public string GetName()
    {
        return _name;
    }
    
    public string GetSurname()
    {
        return _surname;
    }
    
    public string GetEmail()
    {
        return _email;
    }
    
    public string GetPassword()
    {
        return _password;
    }
}