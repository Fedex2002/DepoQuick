using Model.Exceptions;
using Repositories;

namespace Logic;

public class UserLogic
{
    private UserRepositories _userRepositories= new UserRepositories();

    public bool CheckIfEmailIsRegistered(string email)
    {
        if (!_userRepositories.ExistsInRepository(email))
        {
            throw new LogicExceptions("The email is not registered");
        }

        return true;
    }
}