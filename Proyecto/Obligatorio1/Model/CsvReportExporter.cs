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
}