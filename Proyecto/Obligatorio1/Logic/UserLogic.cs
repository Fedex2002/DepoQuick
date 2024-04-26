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

    public User Login(string email,string password)
    {
        return LoginCheckUserValidations(email, password);
    }

    private User LoginCheckUserValidations(string email, string password)
    {
        User user = new User();
        if (CheckIfEmailIsRegistered(email) && CheckIfPasswordIsCorrect(password, _userRepositories.GetFromRepository(email).GetPassword()))
        {
            user = _userRepositories.GetFromRepository(email);
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