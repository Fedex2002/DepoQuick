using Model.Exceptions;
namespace Model;

public class Booking
{
    private bool _approved;
    private DateTime _dateStart;
    private DateTime _dateEnd;
    private StorageUnit _storageUnit;
    private string _rejectedBooking { get; set; }
    
    public Booking()
    {
    }
    
    public Booking(bool approved, DateTime dateStart, DateTime dateEnd, StorageUnit storageUnit, string rejectedBooking)
    {
        _approved = false;
        SetApproved(approved);
        _dateStart = dateStart;
        _dateEnd = dateEnd;
        _storageUnit = storageUnit;
        _rejectedBooking = "";
        SetRejectedBooking(rejectedBooking);
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
    
    public string GetRejectedBooking()
    {
        return _rejectedBooking;
    }
    
    private void SetRejectedBooking(string rejectedBooking)
    {
        _rejectedBooking = rejectedBooking;
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
        double totalPrice = _storageUnit.CalculateStorageUnitPrice() * GetCountOfDays();;
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
        return _rejectedBooking.Length <= 300;
    }
    
    private void SetApproved(bool approved)
    {
        _approved = approved;
        if (!approved)
        {
            throw new BookingExceptions("Approved is not valid");
        }
    }

}
