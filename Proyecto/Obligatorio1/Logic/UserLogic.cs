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
        return _userRepositories.ExistsInRepository(email);
    }

    public void IfEmailIsNotRegisteredThrowException(bool registered)
    {
        if (!registered)
            throw new LogicExceptions("The email is not registered");
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
    
    public void RemoveBookingFromUser(User user, Booking booking)
    {
        user.GetBookings().Remove(booking);
    }

    public User Login(User anyUser)
    {
        return LoginCheckUserValidations(anyUser);
    }

    private User LoginCheckUserValidations(User anyUser)
    {
        User user = new User();
        if (CheckIfEmailIsRegistered(anyUser.GetEmail()) && CheckIfPasswordIsCorrect(anyUser.GetPassword(), _userRepositories.GetFromRepository(anyUser).GetPassword()))
        {
            user = anyUser;
        }

        return user;
    }

    public UserRepositories GetRepository()
    {
        return _userRepositories;
    }

    public void SignUp(User user)
    {
        if (!CheckIfEmailIsRegistered(user.GetEmail()))
        {
            _userRepositories.AddToRepository(user);
        }
    }
}