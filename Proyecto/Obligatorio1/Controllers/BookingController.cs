using DataAccess.Context;
using DataAccess.Repository;
using Logic.DTOs;
using Logic.Interfaces;
using Model;
using Model.Enums;
using Model.Exceptions;
namespace Logic;

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
            bool exists = _bookingRepositories.BookingAlreadyExists(newBooking);
            if (!exists)
            {
                _bookingRepositories.AddBooking(newBooking);
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
    
    private StorageUnit ChangeToStorageUnit(StorageUnitDto storageUnitDto)
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

        AreaType areaType = (AreaType)storageUnitDto.Area.Value;
        SizeType sizeType = (SizeType)storageUnitDto.Size.Value;
        return new StorageUnit(storageUnitDto.Id, areaType, sizeType, storageUnitDto.Climatization, promotions, availableDates);
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
            bookingDto.Payment = true;
        }
    }
    
    public List<BookingDto> GetAllBookingsDto()
    {
        List<BookingDto> bookingsDto = new List<BookingDto>();
        foreach (var booking in _bookingRepositories.GetAllBookings())
        {
            bookingsDto.Add(new BookingDto(booking.Approved, booking.DateStart, booking.DateEnd, new StorageUnitDto(booking.StorageUnit.Id, new AreaTypeDto(booking.StorageUnit.Area), new SizeTypeDto(booking.StorageUnit.Size), booking.StorageUnit.Climatization, new List<PromotionDto>(), new List<DateRangeDto>()), booking.RejectedMessage, booking.Status, booking.Payment, booking.PersonEmail));
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
            List<Booking> bookings = _bookingRepositories.GetAllBookings();
            var bookingToApprove = bookings.FirstOrDefault(
                b => b.PersonEmail == userEmail && b.StorageUnit.Id == bookingDto.StorageUnitDto.Id
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
            List<Booking> bookings = _bookingRepositories.GetAllBookings();
            var bookingToReject = bookings.FirstOrDefault(
                b => b.PersonEmail == userEmail && b.StorageUnit.Id == bookingDto.StorageUnitDto.Id
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