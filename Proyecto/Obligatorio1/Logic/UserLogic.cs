using Model;
using Model.Exceptions;
using Repositories;
namespace Logic;

public class UserLogic
{
    private User user;
    private PersonRepositories personRepo;
    
    public UserLogic(PersonRepositories personRepo)
    {
        this.personRepo = personRepo;
    }
    
    public void AddBookingToUser(Person person, Booking booking)
    {
        CheckIfPersonIsAUserAddBooking(person, booking);
    }

    private static void CheckIfPersonIsAUserAddBooking(Person person, Booking booking)
    {
        if (person is User user)
        {
            user.GetBookings().Add(booking);
        }
        else
        {
            throw new LogicExceptions("The person is not a user");
        }
    }
    
    public bool CheckIfBookingIsApproved(Booking booking)
    {
        return booking.GetApproved();
    }
    
    public void RemoveBookingFromUser(Person person, Booking booking)
    {
        CheckIfPersonIsAUserRemoveBooking(person, booking);
    }

    private static void CheckIfPersonIsAUserRemoveBooking(Person person, Booking booking)
    {
        if (person is User user)
        {
            user.GetBookings().Remove(booking);
        }
    }
}