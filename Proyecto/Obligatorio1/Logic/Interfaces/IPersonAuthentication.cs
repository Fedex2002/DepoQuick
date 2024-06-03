using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IPersonAuthentication
{
    public bool CheckIfEmailIsRegistered(string email);
    public bool CheckIfPasswordIsCorrect(string password, string verifyPassword);
    public Person Login(string email, string password);
}