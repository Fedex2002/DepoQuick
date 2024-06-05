using Logic.DTOs;
using Model;
using Model.Exceptions;
using Repositories;
namespace Logic;

public class BookingController
{
    private readonly BookingRepositories _bookingRepositories;
    
    public BookingController(BookingRepositories bookingRepo)
    {
        _bookingRepositories = bookingRepo;
    }
    
    public void AddBooking(PersonDto userDto, BookingDto bookingDto)
    {
        CheckIfAlreadyBookedAndAddBooking(userDto, bookingDto);
    }

    private void CheckIfAlreadyBookedAndAddBooking(PersonDto userDto, BookingDto bookingDto)
    {
        Booking newBooking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment,userDto.Email);
        List<Booking> bookings = _bookingRepositories.GetAllFromRepository();
            bool exists = bookings.Any(booking => booking.StorageUnit.Id == newBooking.StorageUnit.Id);
            if (!exists)
            {
                _bookingRepositories.AddToRepository(newBooking);
            }
            else
            {
                IfUserAlreadyBookTheStorageUnitThrowException();
            }
    }

    private static void IfUserAlreadyBookTheStorageUnitThrowException()
    {
        throw new LogicExceptions("Booking for this StorageUnit already exists");
    }

    public bool CheckIfBookingIsApproved(BookingDto bookingDto)
    {
        return bookingDto.Approved;
    }
    
    public StorageUnit ChangeToStorageUnit(StorageUnitDto storageUnitDto)
    {
        List<Promotion> promotions = new List<Promotion>();
        foreach (var promotionDto in storageUnitDto.Promotions)
        {
            promotions.Add(new Promotion(promotionDto.Label, promotionDto.Discount, promotionDto.DateStart, promotionDto.DateEnd));
        }
        
        List<DateRange> availableDates = new List<DateRange>();
        foreach (var dateRangeDto in storageUnitDto.AvailableDates)
        {
            availableDates.Add(new DateRange(dateRangeDto.StartDate, dateRangeDto.EndDate));
        }
        return new StorageUnit(storageUnitDto.Id, storageUnitDto.Area, storageUnitDto.Size, storageUnitDto.Climatization, promotions, availableDates);
    }
    
    public double CalculateTotalPriceOfBooking(BookingDto bookingDto)
    {
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, ChangeToStorageUnit(bookingDto.StorageUnitDto), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment, bookingDto.UserEmail);
        return booking.CalculateBookingTotalPrice();
    }
    
    public void PayBooking(PersonDto userDto, BookingDto bookingDto)
    {
        List<Booking> bookings = _bookingRepositories.GetAllFromRepository();
        var bookingToPay = bookings.FirstOrDefault(
            b => b.PersonEmail == userDto.Email && b.StorageUnit.Id == bookingDto.StorageUnitDto.Id
        );

        if (bookingToPay != null)
        {
            IfBookingPaymentIsAlreadyTrueThrowException(bookingToPay);
            bookingToPay.Payment = true;
            bookingDto.Payment = true;
        }
    }
    
    public List<BookingDto> GetAllBookingsDto()
    {
        List<BookingDto> bookingsDto = new List<BookingDto>();
        foreach (var booking in _bookingRepositories.GetAllFromRepository())
        {
            bookingsDto.Add(new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd, new StorageUnitDto(booking.StorageUnit.Id, booking.StorageUnit.Area, booking.StorageUnit.Size, booking.StorageUnit.Climatization, new List<PromotionDto>(), new List<DateRangeDto>()), booking.RejectedMessage, booking.Status, booking.Payment, booking.PersonEmail));
        }
        return bookingsDto;
    }

    private static void IfBookingPaymentIsAlreadyTrueThrowException(Booking booking)
    {
        if (booking.Payment)
        {
            throw new LogicExceptions("Booking already paid");
        }
    }
    
    public void ApproveBooking(PersonDto userDto, BookingDto bookingDto)
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
            List<Booking> bookings = _bookingRepositories.GetAllFromRepository();
            var bookingToApprove = bookings.FirstOrDefault(
                b => b.PersonEmail == userDto.Email && b.StorageUnit.Id == bookingDto.StorageUnitDto.Id
            );

            IfBookingStorageUnitIdIsAMatchSetApprovedToTrueAndStatusToCaptured(bookingDto, bookingToApprove,
                bookingDto.StorageUnitDto.Id);
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

    private void IfBookingStorageUnitIdIsAMatchSetApprovedToTrueAndStatusToCaptured(BookingDto bookingDto,
        Booking booking, string oldBookingId)
    {
        if (booking.StorageUnit.Id == oldBookingId)
        {
            booking.Approved = true;
            booking.Status = "Capturado";
            bookingDto.Approved = true;
            bookingDto.Status = "Capturado";
        }
    }

    public void SetRejectionMessage(PersonDto userDto, BookingDto bookingDto, string rejectionMessage)
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
            List<Booking> bookings = _bookingRepositories.GetAllFromRepository();
            var bookingToReject = bookings.FirstOrDefault(
                b => b.PersonEmail == userDto.Email && b.StorageUnit.Id == bookingDto.StorageUnitDto.Id
            );

            IfBookingStorageUnitIdIsAMatchSetRejectedMessageAndChangeStatusToRejected(bookingDto, bookingToReject,
                rejectionMessage);
        }
    }

    private void IfBookingStorageUnitIdIsAMatchSetRejectedMessageAndChangeStatusToRejected(BookingDto bookingDto,
        Booking booking, string rejectionMessage)
    {
        if (booking.StorageUnit.Id == bookingDto.StorageUnitDto.Id)
        {
            booking.RejectedMessage = rejectionMessage;
            booking.Status = "Rechazado";
            bookingDto.RejectedMessage = rejectionMessage;
            bookingDto.Status = "Rechazado";
        }
    }

    private static void IfRejectionMessageIsEmptyThrowException(string rejectionMessage)
    {
        if (rejectionMessage == "")
        {
            throw new LogicExceptions("The rejection message can't be empty.");
        }
    }
}