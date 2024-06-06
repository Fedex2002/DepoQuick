namespace Model;

public abstract class ReportExporter
{
    public static ReportExporter Create(string type)
    {
        return type.ToLower() switch
        {
            "csv" => new CsvReportExporter(),
         
        };
    }
    
}