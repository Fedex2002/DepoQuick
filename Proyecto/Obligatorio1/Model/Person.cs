using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using Model.Exceptions;

namespace Model;

public class Person
{
    private string Name { get; set; }
    private string Surname { get; set; }
    private string Email { get; set; }
    private string Password { get; set; }

    public Person()
    {
        
    }
    public Person(string name, string surname, string email, string password)
    {
        Name = "";
        Surname = "";
        SetNameAndSurname(name, surname);
        Email = "";
        SetEmail(email);
        Password = "";
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

    public string GetName()
    {
        return Name;
    }
    
    public string GetSurname()
    {
        return Surname;
    }
    
    public string GetEmail()
    {
        return Email;
    }
    
    public string GetPassword()
    {
        return Password;
    }
    
    private void SetPassword(string password)
    {
        Password = password;
        IfHasInvalidPasswordThrowException();
    }

    private void IfHasInvalidPasswordThrowException()
    {
        if(!ValidatePassword())
        {
            throw new PersonExceptions("Password is not valid");
        }
    }

    private void SetEmail(string email)
    {
        Email = email;
        IfHasInvalidEmailThrowException();
    }

    private void IfHasInvalidEmailThrowException()
    {
        if(!ValidateEmail())
        {
            throw new PersonExceptions("Email is not valid");
        }
    }

    private void SetNameAndSurname(string name, string surname)
    {
        Name = name;
        Surname = surname;
        IfHasInvalidNameOrSurnameThrowException(); 
    }

    private void IfHasInvalidNameOrSurnameThrowException()
    {
        if(!ValidateNameAndSurname())
        {
            throw new PersonExceptions("Name or Surname are not valid");
        }
    }
}