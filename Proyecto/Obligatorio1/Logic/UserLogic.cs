using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class UserLogic
{
    private PersonRepositories _personRepositories;
    
    public UserLogic(PersonRepositories personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public bool CheckIfEmailIsRegistered(string email)
    {
        return _personRepositories.ExistsInRepository(email);
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
        if (CheckIfEmailIsRegistered(email) && CheckIfPasswordIsCorrect(password, _personRepositories.GetFromRepository(email).GetPassword()))
        {
            user = _personRepositories.GetFromRepository(email);
        }
        else
        {
            throw new LogicExceptions("The user does not exist");
        }

        return user;
    }

    public PersonRepositories GetRepository()
    {
        return _personRepositories;
    }

    public void SignUp(User user)
    {
        if (!CheckIfEmailIsRegistered(user.GetEmail()))
        {
            _personRepositories.AddToRepository(user);
        }
    }
}