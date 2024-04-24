using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class UserLogic
{
    private UserRepositories _userRepositories;
    
    public UserLogic(UserRepositories userRepositories)
    {
        _userRepositories = userRepositories;
    }

    public bool CheckIfEmailIsRegistered(string email)
    {
        if (!_userRepositories.ExistsInRepository(email))
            throw new LogicExceptions("The email is not registered");
        return true;
    }

    public bool CheckIfPasswordIsCorrect(string userpassword, string catchFromPage)
    {
       
        if (PasswordStringMatch(userpassword, catchFromPage))
            throw new LogicExceptions("The password is not correct");
        return true;
    }

    private static bool PasswordStringMatch(string userpassword, string catchFromPage)
    {
        return userpassword != catchFromPage;
    }
    
    public void AddBookingToUser(User user, Booking booking)
    {
        user.GetBookings().Add(booking);
    }

    public bool ApprovedBooking(Booking booking)
    {
        return booking.GetApproved();
    }
}