namespace Logic.DTOs;

public class BookingDto
{
    public bool Approved { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public StorageUnitDto StorageUnit { get; set; }
    public string RejectedBooking { get; set; }

    public BookingDto()
    {
    }

    public BookingDto( bool approved, DateTime dateStart, DateTime dateEnd, StorageUnitDto storageUnit, string rejectedBooking)
    {
        Approved = approved;
        DateStart = dateStart;
        DateEnd = dateEnd;
        StorageUnit = storageUnit;
        RejectedBooking = rejectedBooking;
    }
  
}