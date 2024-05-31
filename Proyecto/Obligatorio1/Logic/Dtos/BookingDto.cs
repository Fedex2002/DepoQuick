namespace Logic.DTOs;

public class BookingDto
{
    public bool Approved { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public StorageUnitDto StorageUnitDto { get; set; }
    public string RejectedMessage { get; set; }

    public string UserEmail { get; set; }

    public BookingDto()
    {
    }

    public BookingDto( bool approved, DateTime dateStart, DateTime dateEnd, StorageUnitDto storageUnitDto, string rejectedMessage, string userEmail)
    {
        Approved = approved;
        DateStart = dateStart;
        DateEnd = dateEnd;
        StorageUnitDto = storageUnitDto;
        RejectedMessage = rejectedMessage;
        UserEmail = userEmail;
    }
  
}