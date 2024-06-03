namespace Model.Interfaces;

public interface IReportExporter 
{
    public string Export(List<Booking> bookings);
    public string GetData(List<Booking> bookings);
}