using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;

namespace Logic;

public class PersonLogic
{
    private readonly PersonRepositories _personRepositories;
    
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
            if (CheckIfPasswordIsCorrect(password, person.Password))
            {
                personDto = new PersonDto(person.Name, person.Surname, person.Email, person.Password,person.IsAdmin);
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
            CheckIfIsUserOrAdministratorAndAddToTheRepository(personDto);
        }
        else
        {
            throw new LogicExceptions("The email is already registered");
        }
    }

    private void CheckIfIsUserOrAdministratorAndAddToTheRepository(PersonDto personDto)
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
        }
    }

    public List<BookingDto> ChangeToBookingsDtos(List<Booking> bookings)
    {
        List<BookingDto> bookingDtos = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            List<PromotionDto> promotionDtos = new List<PromotionDto>();
            foreach (var promotion in booking.StorageUnit.Promotions)
            {
                promotionDtos.Add(new PromotionDto(promotion.Label, promotion.Discount, promotion.DateStart, promotion.DateEnd));
            }
            
            List<DateRangeDto> availableDates = new List<DateRangeDto>();
            foreach (var dateRange in booking.StorageUnit.AvailableDates)
            {
                availableDates.Add(new DateRangeDto(dateRange.StartDate, dateRange.EndDate));
            }
            StorageUnitDto storageUnitDto = new StorageUnitDto(booking.StorageUnit.Id, booking.StorageUnit.Area, booking.StorageUnit.Size, booking.StorageUnit.Climatization, promotionDtos, availableDates);
            bookingDtos.Add(new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd, storageUnitDto,
                booking.RejectedMessage, booking.Status, booking.Payment));
        }

        return bookingDtos;
    }
}
    