namespace Model;

public class Booking
{
    private bool approved;
    private DateTime dateStart;
    private DateTime dateEnd;
    private StorageUnit storageUnit;
    
    public Booking()
    {
    }
    
    public Booking(bool approved, DateTime dateStart, DateTime dateEnd, StorageUnit storageUnit)
    {
        this.approved = approved;
        this.dateStart = dateStart;
        this.dateEnd = dateEnd;
        this.storageUnit = storageUnit;
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

    public int GetCountOfDays()
    {
        return (dateEnd - dateStart).Days;
    }
    
    public double CalculateBookingTotalPrice()
    {
        return storageUnit.CalculateStorageUnitPrice() * GetCountOfDays();
    }
    
}
