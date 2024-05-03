using Model.Exceptions;
namespace Model;

public class Booking
{
    private bool _approved;
    private DateTime _dateStart;
    private DateTime _dateEnd;
    private StorageUnit _storageUnit;
    private string _rejectedMessage { get; set; }
    
    public Booking()
    {
    }
    
    public Booking(bool approved, DateTime dateStart, DateTime dateEnd, StorageUnit storageUnit, string rejectedMessage)
    {
        _approved = false;
        SetApproved(approved);
        _dateStart = DateTime.MinValue;
        _dateEnd = DateTime.MaxValue;
        SetDate(dateStart, dateEnd);
        _storageUnit = storageUnit;
        _rejectedMessage = "";
        SetRejectedBooking(rejectedMessage);
    }
    
    public bool GetApproved()
    {
        return _approved;
    }
    
    public DateTime GetDateStart()
    {
        return _dateStart;
    }
    
    public DateTime GetDateEnd()
    {
        return _dateEnd;
    }
    
    public string GetRejectedMessage()
    {
        return _rejectedMessage;
    }
    
    public StorageUnit GetStorageUnit()
    {
        return _storageUnit;
    }
    
    private void SetRejectedBooking(string rejectedMessage)
    {
        _rejectedMessage = rejectedMessage;
        IfHasInvalidRejectionThrowException();
    }

    private void IfHasInvalidRejectionThrowException()
    {
        if (!CheckRejection())
        {
            throw new BookingExceptions("Rejection message is not valid");
        }
    }

    public int GetCountOfDays()
    {
        return (_dateEnd - _dateStart).Days;
    }
    
    public double CalculateBookingTotalPrice()
    {
        return TotalPriceWithDiscountForBookingDays();
    }

    private double TotalPriceWithDiscountForBookingDays()
    {
        double totalPrice = _storageUnit.CalculateStorageUnitPricePerDay() * GetCountOfDays();;
        return CheckDiscount(totalPrice);
    }

    private double CheckDiscount(double totalPrice)
    {
        double discount = 0;
        double totalPriceWithDiscount = 0;
        if (GetCountOfDays() >= 7 && GetCountOfDays() <= 14)
        {
            discount = 5;
            totalPriceWithDiscount = RuleOf3(totalPrice, discount);
        } else if (GetCountOfDays() > 14)
        {
            discount = 10;
            totalPriceWithDiscount = RuleOf3(totalPrice, discount);
        }
        else
        {
            totalPriceWithDiscount = totalPrice;
        }

        return totalPriceWithDiscount;
    }

    private double RuleOf3(double totalPrice, double discount)
    {
        double totalDiscount = (totalPrice * discount) / 100;
        return totalPrice - totalDiscount;
    }
    
    public bool CheckRejection()
    {
        return _rejectedMessage.Length <= 300;
    }

    private void SetApproved(bool approved)
    {
        _approved = approved;
    }

    public bool CheckDate()
    {
        return _dateStart < _dateEnd;
    }
    
    private void SetDate(DateTime dateStart, DateTime dateEnd)
    {
        _dateStart = dateStart;
        _dateEnd = dateEnd;
        IfHasInvalidDateThrowException();
    }

    private void IfHasInvalidDateThrowException()
    {
        if (!CheckDate())
        {
            throw new BookingExceptions("Date is not valid");
        }
    }
}
