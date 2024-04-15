namespace Model;

public class Booking
{
    private bool _approved;
    private DateTime _dateStart;
    private DateTime _dateEnd;
    private StorageUnit _storageUnit;
    private string _rejectedBooking;
    
    public Booking()
    {
    }
    
    public Booking(bool approved, DateTime dateStart, DateTime dateEnd, StorageUnit storageUnit, string rejectedBooking)
    {
        this._approved = approved;
        this._dateStart = dateStart;
        this._dateEnd = dateEnd;
        this._storageUnit = storageUnit;
        this._rejectedBooking = rejectedBooking;
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
}
