using Repositories;
using Logic.DTOs;
using Model;
using Model.Exceptions;
namespace Logic;

public class AdministratorLogic
{
    private readonly PersonRepositories _personRepositories;

    public AdministratorLogic(PersonRepositories personRepositories)
    {
        _personRepositories = personRepositories;
    }

    public void ApproveBooking(UserDto userDto, BookingDto bookingDto)
    {
        if (bookingDto.Approved)
        {
            IfBookingIsAlreadyApprovedThrowException();
        }
        else if (bookingDto.RejectedMessage != "")
        {
            IfBookingRejectedMessageIsNotEmptyThrowException();
        } 
        else if (bookingDto.Payment == false)
        {
            IfUserDidNotMakeThePaymentThrowException();
        }
        else
        {
            string oldBookingStorageUnitId = bookingDto.StorageUnitDto.Id;
            Person person = _personRepositories.GetFromRepository(userDto.Email);
            if (person is User user)
            {
                var userBookings = user.Bookings.ToList();
                foreach (var booking in userBookings)
                { 
                    IfBookingStorageUnitIdIsAMatchSetApprovedToTrueAndStatusToCaptured(booking, oldBookingStorageUnitId);
                }
            }
        }
    }

    private static void IfUserDidNotMakeThePaymentThrowException()
    {
        throw new LogicExceptions("The booking can't be approved without user payment");
    }

    private static void IfBookingIsAlreadyApprovedThrowException()
    {
        throw new LogicExceptions("Booking is already approved");
    }

    private static void IfBookingRejectedMessageIsNotEmptyThrowException()
    {
        throw new LogicExceptions("Booking is already rejected");
    }

    private void IfBookingStorageUnitIdIsAMatchSetApprovedToTrueAndStatusToCaptured(Booking booking, string oldBookingId)
    {
        if (booking.StorageUnit.Id == oldBookingId)
        {
            booking.Approved = true;
            booking.Status = "Capturado";
        }
    }

    public void SetRejectionMessage(UserDto userDto, BookingDto bookingDto, string rejectionMessage)
    {
        if (bookingDto.RejectedMessage != "")
        {
            IfBookingRejectedMessageIsNotEmptyThrowException();
        }
        else if (bookingDto.Approved)
        {
            IfBookingIsAlreadyApprovedThrowException();
        }
        else if (bookingDto.Payment == false)
        {
            IfUserDidNotMakeThePaymentThrowException();
        }
        else
        {
            IfRejectionMessageIsEmptyThrowException(rejectionMessage);
            string oldBooking = bookingDto.StorageUnitDto.Id;
            Person person = _personRepositories.GetFromRepository(userDto.Email);
            if (person is User user)
            {
                var userBookings = user.Bookings.ToList();
                foreach (var booking in userBookings)
                {
                    IfBookingStorageUnitIdIsAMatchSetRejectedMessageAndChangeStatusToRejected(booking, oldBooking, rejectionMessage);
                }
            }
        }
    }

    private void IfBookingStorageUnitIdIsAMatchSetRejectedMessageAndChangeStatusToRejected(Booking booking, string oldBookingId, string rejectionMessage)
    {
        if (booking.StorageUnit.Id == oldBookingId)
        {
            booking.RejectedMessage = rejectionMessage;
            booking.Status = "Rechazado";
        }
    }
    
    private static void IfRejectionMessageIsEmptyThrowException(string rejectionMessage)
    {
        if (rejectionMessage == "")
        {
            throw new LogicExceptions("The rejection message can't be empty.");
        }
    }

    public List<UserDto> GetUsersDto()
    {
        List<Person> users = _personRepositories.GetAllFromRepository();
        List<UserDto> usersDto = new List<UserDto>();
        foreach (var person in users)
        {
            if (person is User user)
            {
                UserDto userDto = new UserDto(user.Name, user.Surname, user.Email, user.Password, 
                    GetUserBookingsDto(user.Bookings));
                usersDto.Add(userDto);
            }
        }

        return usersDto;
    }

    private List<BookingDto> GetUserBookingsDto(List<Booking> bookings)
    {
        List<BookingDto> bookingsDto = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            BookingDto bookingDto = new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd,
                GetUserStorageUnitDto(booking.StorageUnit), booking.RejectedMessage, booking.Status, booking.Payment,booking.PersonEmail);
            bookingsDto.Add(bookingDto);
        }
        return bookingsDto;
    }
    
    private StorageUnitDto GetUserStorageUnitDto(StorageUnit storageUnit)
    {
        return new StorageUnitDto(storageUnit.Id, storageUnit.Area, storageUnit.Size,
            storageUnit.Climatization, GetUserPromotionsDto(storageUnit.Promotions), GetDateRangesDto(storageUnit.AvailableDates));
    }

    private List<PromotionDto> GetUserPromotionsDto(List<Promotion> promotions)
    {
        List<PromotionDto> promotionsDto = new List<PromotionDto>();
        foreach (var promotion in promotions)
        {
            PromotionDto promotionDto = new PromotionDto(promotion.Label, promotion.Discount,
                promotion.DateStart, promotion.DateEnd);
            promotionsDto.Add(promotionDto);
        }

        return promotionsDto;
    }
    
    private List<DateRangeDto> GetDateRangesDto(List<DateRange> dateRanges)
    {
        List<DateRangeDto> dateRangesDto = new List<DateRangeDto>();
        foreach (var dateRange in dateRanges)
        {
            DateRangeDto dateRangeDto = new DateRangeDto(dateRange.StartDate, dateRange.EndDate);
            dateRangesDto.Add(dateRangeDto);
        }

        return dateRangesDto;
    }

    public List<Booking> GetAllUserBookings()
    {
        List<Person> users = _personRepositories.GetAllFromRepository();
        List<Booking> allBookings = new List<Booking>();

        foreach (var person in users)
        {
            if (person is User user)
            {
                allBookings.AddRange(user.Bookings);
            }
        }

        return allBookings;
    }

    public void ExportToCsv(string filePath)
    {
        List<Booking> bookings = GetAllUserBookings();
        CsvReportExporter csvReportExporter = new CsvReportExporter();
        csvReportExporter.Export(filePath, bookings);
    }
}