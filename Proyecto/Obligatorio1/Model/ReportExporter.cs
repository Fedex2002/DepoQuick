namespace Model;

public abstract class ReportExporter
{
    public abstract string Export(List<Booking> bookings);
    
    public static ReportExporter Create(string type)
    {
        return type.ToLower() switch
        {
            "csv" => new CsvReportExporter(),
            "txt" => new TxtReportExporter(),
            _ => throw new ArgumentException("Invalid type", nameof(type))
        };
    }
    
}