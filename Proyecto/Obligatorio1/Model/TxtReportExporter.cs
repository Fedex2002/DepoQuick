using System.Text;

namespace Model;

public class TxtReportExporter : ReportExporter
{
    public TxtReportExporter()
    {
    }

    public override string Export(List<Booking> bookings)
    {
        return GetData(bookings);
    }

    public string GetData(List<Booking> bookings)
    {
        var txtBuilder = new StringBuilder();
        foreach (var booking in bookings)
        {
            txtBuilder.AppendLine($"StorageUnit Id: {booking.StorageUnit.Id}");
            txtBuilder.AppendLine($"Area: {booking.StorageUnit.Area}");
            txtBuilder.AppendLine($"Size: {booking.StorageUnit.Size}");
            txtBuilder.AppendLine($"Climatization: {booking.StorageUnit.Climatization}");
            txtBuilder.AppendLine($"StartDate: {booking.DateStart:yyyy-MM-dd}");
            txtBuilder.AppendLine($"EndDate: {booking.DateEnd:yyyy-MM-dd}");
            txtBuilder.AppendLine($"Status: {booking.Status}");
            txtBuilder.AppendLine();
        }

        return txtBuilder.ToString();
    }
}