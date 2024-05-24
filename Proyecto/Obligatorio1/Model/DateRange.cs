namespace Model;

public class DateRange
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateRange()
    {
        
    }
    
    public DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}