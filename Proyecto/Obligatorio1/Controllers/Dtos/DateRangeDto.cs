namespace Controllers.Dtos;

public class DateRangeDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateRangeDto()
    {
    }

    public DateRangeDto(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}