using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using Model.Exceptions;

namespace Model;

public class Person
{

    private string _name;
    private string _surname;
    private string _email;
    private string _password;

    public Person()
    {
        
    }
    public Person(string name, string surname, string email, string password)
    {

        Name = name;
        Surname = surname;
        Email = email;
        Password = password;

    }
    
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    public string Surname
    {
        get => _surname;
        set
        {
            _surname = value;
            IfHasInvalidNameOrSurnameThrowException();
        }
    }
    
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            IfHasInvalidEmailThrowException();
        }
    }
    
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            IfHasInvalidPasswordThrowException();
        }
    }
    
    
    
    public bool ValidatePassword()
    {
        return HasCorrectNumberOfDigits() && HasUppercaseLetter() && HasLowercaseLetter() 
               && HasAtLeastOneSymbol() && HasAtLeastOneNumber();
    }
    
    public bool ValidateEmail()
    {
        string pattern = @"^[a-zA-Z0-9.%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(this.Email, pattern);
    }
    
    public bool ValidateNameAndSurname()
    {
        return CheckIfEmpty() && CheckLength() && CheckPattern();
    }
    
    private bool CheckIfEmpty()
    {
        return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Surname);
    }
    
    private bool CheckLength()
    {
        return Name.Length + Surname.Length <= 100;
    }
    
    private bool CheckPattern()
    {
        string pattern = "^[a-zA-Z ]+$";
        return Regex.IsMatch(Name, pattern) && Regex.IsMatch(Surname, pattern);
    }
    
    private bool HasCorrectNumberOfDigits()
    {
        return Password.Length >= 8;
    }

    private bool HasUppercaseLetter()
    {
        bool ret = false;
        foreach (var p in Password)
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
        foreach (var p in Password)
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
            if (Password.Contains(symbol))
            {
                ret = true;
            }
        }

        return ret;
    }

    private bool HasAtLeastOneNumber()
    {
        bool ret = false;
        foreach (var p in Password)
        {
            if (char.IsDigit(p))
            {
                ret = true;
            }
        }

        return ret;
    }

    

    private void IfHasInvalidPasswordThrowException()
    {
        if(!ValidatePassword())
        {
            throw new PersonExceptions("Password is not valid");
        }
    }

    

    private void IfHasInvalidEmailThrowException()
    {
        if(!ValidateEmail())
        {
            throw new PersonExceptions("Email is not valid");
        }
    }



    private void IfHasInvalidNameOrSurnameThrowException()
    {
        if(!ValidateNameAndSurname())
        {
            throw new PersonExceptions("Name or Surname are not valid");
        }
    }
}