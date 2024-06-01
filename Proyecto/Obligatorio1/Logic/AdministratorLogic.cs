using Repositories;
using Logic.DTOs;
using Model;
using Model.Exceptions;
using RepositoriesInterface;

namespace Logic;

public class AdministratorLogic
{
    private readonly BookingRepositories _bookingRepositories;

    public AdministratorLogic(BookingRepositories bookingRepo)
    {
        _bookingRepositories = bookingRepo;
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

    
 
    
    public List<Booking> GetAllUserBookings()
    {
        return _bookingRepositories.GetAllFromRepository();
    }
    
    public void ExportToCsv(string filePath)
    {
        List<Booking> bookings = GetAllUserBookings();
        CsvReportExporter csvReportExporter = new CsvReportExporter();
        csvReportExporter.Export(filePath, bookings);
    }
    
}