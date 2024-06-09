using Controllers.Dtos;
using Controllers.Interfaces;
using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Exceptions;

namespace Controllers;

public class BookingController : IBookingController
{
    private readonly BookingsRepository _bookingRepositories;
    private readonly StorageUnitsRepository _storageUnitsRepository;
    public BookingController(ApplicationDbContext context)
    {
        _bookingRepositories = new BookingsRepository(context);
        _storageUnitsRepository = new StorageUnitsRepository(context);
    }
    
    public void CreateBooking(string userEmail, BookingDto bookingDto)
    {
        CheckIfAlreadyBookedAndAddBooking(userEmail, bookingDto);
    }

    private void CheckIfAlreadyBookedAndAddBooking(string userEmail, BookingDto bookingDto)
    {
        Booking newBooking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, _storageUnitsRepository.GetStorageUnitFromId(bookingDto.StorageUnitDto.Id), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment,userEmail);
        _bookingRepositories.AddBooking(newBooking);
    }

    private static void IfUserAlreadyBookTheStorageUnitThrowException()
    {
        throw new LogicExceptions("Booking for this StorageUnit already exists");
    }

    public bool CheckIfBookingIsApproved(BookingDto bookingDto)
    {
        return bookingDto.Approved;
    }
    
  
    
    public double CalculateTotalPriceOfBooking(BookingDto bookingDto)
    {
        Booking booking = new Booking(bookingDto.Approved, bookingDto.DateStart, bookingDto.DateEnd, _storageUnitsRepository.GetStorageUnitFromId(bookingDto.StorageUnitDto.Id), bookingDto.RejectedMessage, bookingDto.Status, bookingDto.Payment, bookingDto.UserEmail);
        return booking.CalculateBookingTotalPrice();
    }
    
    public void PayBooking(string userEmail, BookingDto bookingDto)
    {
        
        Booking bookingToPay = _bookingRepositories.FindBookingByStorageUnitIdAndEmail(bookingDto.StorageUnitDto.Id, userEmail);
        if (bookingToPay != null)
        {
            IfBookingPaymentIsAlreadyTrueThrowException(bookingToPay);
            bookingToPay.Payment = true;
            _bookingRepositories.UpdateBooking(bookingToPay);
        }
    }
    
    public List<BookingDto> GetAllBookingsDto()
    {
        List<BookingDto> bookingsDto = new List<BookingDto>();
        foreach (var booking in _bookingRepositories.GetAllBookings())
        {
            var storageUnit = _storageUnitsRepository.GetStorageUnitFromId(booking.StorageUnit.Id); 
            var promotionDtos = storageUnit.Promotions.Select(p => new PromotionDto(p.Label, p.Discount, p.DateStart, p.DateEnd)).ToList();
            var dateRangeDtos = storageUnit.AvailableDates.Select(dr => new DateRangeDto(dr.StartDate, dr.EndDate)).ToList();

            var storageUnitDto = new StorageUnitDto(storageUnit.Id, 
                new AreaTypeDto(storageUnit.Area), 
                new SizeTypeDto(storageUnit.Size), 
                storageUnit.Climatization, 
                promotionDtos, 
                dateRangeDtos);

            bookingsDto.Add(new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd, storageUnitDto, booking.RejectedMessage, booking.Status, booking.Payment, booking.PersonEmail));
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
    
    public void ApproveBooking(string userEmail, BookingDto bookingDto)
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
            var bookingToApprove = _bookingRepositories.FindBookingByStorageUnitIdAndEmail(bookingDto.StorageUnitDto.Id, userEmail);
            bookingToApprove.Approved = true;
            bookingToApprove.Status = "Capturado";
            _bookingRepositories.UpdateBooking(bookingToApprove);
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

    public void SetRejectionMessage(string userEmail, BookingDto bookingDto, string rejectionMessage)
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
            Booking booking = _bookingRepositories.FindBookingByStorageUnitIdAndEmail(bookingDto.StorageUnitDto.Id, userEmail);
            booking.RejectedMessage = rejectionMessage;
            booking.Status = "Rechazado";
            _bookingRepositories.UpdateBooking(booking);
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