using Model;
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
        if (person is User user)
        {
            user.GetBookings().Add(booking);
        }
    }
}