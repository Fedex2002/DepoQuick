using Model.Exceptions;
using Repositories;

namespace Logic;

public class UserLogic
{
    private UserRepositories _userRepositories;

    public bool CheckIfEmailIsRegistered(string email)
    {
       throw new LogicExceptions("The email is not registered");
    }
}