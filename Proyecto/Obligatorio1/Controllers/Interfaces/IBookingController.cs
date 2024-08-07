using Controllers.Dtos;

namespace Controllers.Interfaces;

public interface IBookingController
{
    public void CreateBooking(string userEmail, BookingDto bookingDto);
    public double CalculateTotalPriceOfBooking(BookingDto bookingDto);
    public void PayBooking(string userEmail, BookingDto bookingDto);
    public List<BookingDto> GetAllBookingsDto();
    public void ApproveBooking(string userEmail, BookingDto bookingDto);
    public void SetRejectionMessage(string userEmail, BookingDto bookingDto, string rejectionMessage);
    
}