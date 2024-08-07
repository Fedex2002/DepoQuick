using Controllers.Interfaces;
using DataAccess.Context;
using DataAccess.Repository;
using Model;

namespace Controllers;

public class ReportExportController : IReportExportController
{
    private readonly BookingsRepository _bookingRepositories;
    private ReportExporter _reportExporter;
    public ReportExportController(ApplicationDbContext context)
    {
        _bookingRepositories = new BookingsRepository(context);
    }
    
    public string Export(string format)
    {
        List<Booking> bookings = _bookingRepositories.GetAllBookings();
        _reportExporter = ReportExporter.Create(format);
        return _reportExporter.Export(bookings);
    }
    
    
}