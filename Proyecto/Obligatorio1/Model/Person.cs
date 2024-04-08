using Model.Exceptions;

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
        _password = "";
        SetPassword(password);
    }
    
    public bool ValidatePassword()
    {
        return HasCorrectNumberOfDigits() && HasUppercaseLetter() && HasLowercaseLetter() 
               && HasAtLeastOneSymbol() && HasAtLeastOneNumber();
    }

    private bool HasCorrectNumberOfDigits()
    {
        return _password.Length >= 8;
    }

    private bool HasUppercaseLetter()
    {
        foreach (var p in _password)
        {
            if (char.IsUpper(p))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasLowercaseLetter()
    {
        foreach (var p in _password)
        {
            if (char.IsLower(p))
            {
                return true;
            }

        }

        return false;
    }

    private bool HasAtLeastOneSymbol()
    {
        char[] symbols = { '#', '@', '$', '.', ',' };
        foreach (char symbol in symbols)
        {
            if (_password.Contains(symbol))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasAtLeastOneNumber()
    {
        foreach (var p in _password)
        {
            if (char.IsDigit(p))
            {
                return true;
            }
        }

        return false;
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
    
    private void SetPassword(string password)
    {
        _password = password;
        if(!ValidatePassword())
        {
            throw new PersonExceptions("Password is not valid");
        } 
     
        
    }
    
}