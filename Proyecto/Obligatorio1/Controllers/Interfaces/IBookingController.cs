using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IBookingController
{
    public void CreateBooking(string userEmail, BookingDto bookingDto);
    public bool CheckIfBookingIsApproved(BookingDto bookingDto);
    public void ApproveBooking(PersonDto personDto, BookingDto bookingDto);
    public void SetRejectionMessage(PersonDto personDto, BookingDto bookingDto, string rejectionMessage);
    public void AddUserToBooking(BookingDto bookingDto, PersonDto personDto);
    public double CalculateTotalPriceOfBooking(BookingDto bookingDto);
    public void PayBooking(PersonDto personDto, BookingDto bookingDto);

}