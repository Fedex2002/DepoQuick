namespace Model;

public class Booking
{
    private bool approved;
    private DateTime dateStart;
    private DateTime dateEnd;
    
    public Booking()
    {
    }
    
    public Booking(bool approved, DateTime dateStart, DateTime dateEnd)
    {
        this.approved = approved;
        this.dateStart = dateStart;
        this.dateEnd = dateEnd;
    }
    
    public bool GetApproved()
    {
        return approved;
    }
    
    public DateTime GetDateStart()
    {
        return dateStart;
    }
    
    public DateTime GetDateEnd()
    {
        return dateEnd;
    }
}
