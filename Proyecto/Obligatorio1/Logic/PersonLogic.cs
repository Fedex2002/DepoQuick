using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class PersonLogic
{
    private PersonRepositories _personRepositories;
    
    public PersonLogic(PersonRepositories personRepositories)
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
    
    public bool CheckIfPasswordIsCorrect(string personpass, string catchFromPage)
    {
       
        if (PasswordStringMatch(personpass, catchFromPage))
            throw new LogicExceptions("The password is not correct");
        return true;
    }

    private static bool PasswordStringMatch(string personpass, string catchFromPage)
    {
        return personpass != catchFromPage;
    }
    


    public PersonDto Login(string email, string password)
    {
        return LoginCheckPersonValidations(email, password);
    }

    private PersonDto LoginCheckPersonValidations(string email, string password)
    {
        PersonDto personDto = new PersonDto();
        if (CheckIfEmailIsRegistered(email))
        {
            Person person = _personRepositories.GetFromRepository(email);
            if (CheckIfPasswordIsCorrect(password, person.GetPassword()))
            {
                if (person is Administrator)
                {
                    personDto= new AdministratorDto(person.GetName(), person.GetSurname(), person.GetEmail(), person.GetPassword());
                }
                else if (person is User user)
                {
                    personDto= new UserDto(user.GetName(), user.GetSurname(), user.GetEmail(), user.GetPassword(), ChangeToBookingsDtos(user.GetBookings()));
                }
                else if (person != null)
                {
                    personDto = new PersonDto(person.GetName(), person.GetSurname(), person.GetEmail(),
                        person.GetPassword());
                }

            }
        }
        else
        {
            throw new LogicExceptions("The email is not registered");
        }

        return personDto;
    }

    public PersonRepositories GetRepository()
    {
        return _personRepositories;
    }

    public void SignUp(PersonDto personDto)
    {
        if (!CheckIfEmailIsRegistered(personDto.Email))
        {
            if (personDto is UserDto userDto)
            {
                User user = new User(userDto.Name, userDto.Surname, userDto.Email, userDto.Password, new List<Booking>());
                _personRepositories.AddToRepository(user);
            }
            else 
            {
                if(personDto is AdministratorDto adminDto)
                {
                    Administrator admin = new Administrator(adminDto.Name, adminDto.Surname, adminDto.Email, adminDto.Password);
                    _personRepositories.AddToRepository(admin);
                }
                else
                {
                    Person person = new Person(personDto.Name, personDto.Surname, personDto.Email, personDto.Password);
                    _personRepositories.AddToRepository(person);
                }
            }
        }
        else
        {
            throw new LogicExceptions("The email is already registered");
        }
    }
    
    public List<BookingDto> ChangeToBookingsDtos(List<Booking> bookings)
    {
        List<BookingDto> bookingDtos = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            List<PromotionDto> promotionDtos = new List<PromotionDto>();
            foreach (var Promotion in booking.GetStorageUnit().GetPromotions())
            {
                promotionDtos.Add(new PromotionDto(Promotion.GetLabel(), Promotion.GetDiscount(), Promotion.GetDateStart(), Promotion.GetDateEnd()));
            }
            StorageUnitDto storageUnitDto = new StorageUnitDto(booking.GetStorageUnit().GetId(), booking.GetStorageUnit().GetArea(), booking.GetStorageUnit().GetSize(), booking.GetStorageUnit().GetClimatization(), promotionDtos);
            bookingDtos.Add(new BookingDto(booking.GetApproved(), booking.GetDateStart(), booking.GetDateEnd(), storageUnitDto, booking.GetRejectedMessage()));
        }

        return bookingDtos;
    }
}
    