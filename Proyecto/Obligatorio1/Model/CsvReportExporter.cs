using System.Text;
using Model.Interfaces;

namespace Model;

public class CsvReportExporter : IReportExporter
{

    public CsvReportExporter()
    {
    }

    public string Export(List<Booking> bookings)
    {
        return GetData(bookings);
    }
    
    public string GetData(List<Booking> bookings)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("StorageUnit Id,Area,Size,Climatization,StartDate,EndDate,Status");
        foreach (var booking in bookings)
        {
            csvBuilder.AppendLine(
                $"\"{booking.StorageUnit.Id}\"," +
                $"\"{booking.StorageUnit.Area}\"," +
                $"\"{booking.StorageUnit.Size}\"," +
                $"\"{booking.StorageUnit.Climatization}\"," +
                $"\"{booking.DateStart:yyyy-MM-dd}\"," +
                $"\"{booking.DateEnd:yyyy-MM-dd}\"," +
                $"\"{booking.Status}\""
            );
        }
        return csvBuilder.ToString();
    }
}