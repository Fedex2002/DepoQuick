namespace Model;

public class CsvReportExporter
{

    public CsvReportExporter()
    {
    }

    public void Export(string path, List<Booking> bookings)
    {
        File.WriteAllText(path,"");
    }
    
    public string GetData(List<Booking> bookings)
    {
        string data = "DEPOSITO,RESERVA,PAGO\r\n";
        foreach (var booking in bookings)
        {
            data += booking.StorageUnit.Id +"," +booking.StorageUnit.Area+ "," +booking.StorageUnit.Climatization+ "," +booking.StorageUnit.AvailableDates.Count + "," +booking.StorageUnit.Size+ "," +booking.StorageUnit.Promotions.Count + "," + booking.Approved + "," + booking.DateStart + "," + booking.DateEnd + "," + booking.RejectedMessage + "," + booking.Status + "," + booking.Payment + "\r\n";
        }
        return data;
    }
}