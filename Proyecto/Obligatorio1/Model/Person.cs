using System.Text.RegularExpressions;
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
        _email = "";
        SetEmail(email);
        _password = "";
        SetPassword(password);
    }
    
    public bool ValidatePassword()
    {
        return HasCorrectNumberOfDigits() && HasUppercaseLetter() && HasLowercaseLetter() 
               && HasAtLeastOneSymbol() && HasAtLeastOneNumber();
    }
    
    public bool ValidateEmail()
    {
        string pattern = @"^[a-zA-Z0-9.%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(this._email, pattern);
    }
    
    private bool HasCorrectNumberOfDigits()
    {
        return _password.Length >= 8;
    }

    private bool HasUppercaseLetter()
    {
        bool ret = false;
        foreach (var p in _password)
        {
            if (char.IsUpper(p))
            {
                ret = true;
                
            }
        }

        return ret;
    }

    private bool HasLowercaseLetter()
    {
        bool ret = false;
        foreach (var p in _password)
        {
            if (char.IsLower(p))
            {
                ret = true;
                
            }

        }

        return ret;
    }

    private bool HasAtLeastOneSymbol()
    {
        bool ret = false;
        char[] symbols = { '#', '@', '$', '.', ',' };
        foreach (char symbol in symbols)
        {
            if (_password.Contains(symbol))
            {
                ret = true;
            }
        }

        return ret;
    }

    private bool HasAtLeastOneNumber()
    {
        bool ret = false;
        foreach (var p in _password)
        {
            if (char.IsDigit(p))
            {
                ret = true;
            }
        }

        return ret;
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
    
    private void SetEmail(string email)
    {
        _email = email;
        if(!ValidateEmail())
        {
            throw new PersonExceptions("Email is not valid");
        } 
        
    }
    
    
}