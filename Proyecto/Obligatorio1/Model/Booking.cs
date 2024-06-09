using Model.Exceptions;
namespace Model;

public class Booking
{
    private bool _approved;
    private DateTime _dateStart = DateTime.MinValue;
    private DateTime _dateEnd = DateTime.MaxValue;
    private StorageUnit _storageUnit;

    private string _status = "Reservado";
    private bool _payment;
    private string _rejectedMessage;
    private string _personEmail;
    
    public Booking()
    {
    }
    
    public Booking(bool approved, DateTime dateStart, DateTime dateEnd, StorageUnit storageUnit, string rejectedMessage, string status, bool payment,string personEmail)
    {
        Approved = approved;
        DateStart = dateStart;
        DateEnd = dateEnd;
        StorageUnit = storageUnit;
        RejectedMessage = rejectedMessage;
        Status = status;
        Payment = payment;
        PersonEmail = personEmail;
    }
    
    
    public bool Approved
    {
        get => _approved;
        set => _approved = value;
    }
    
    public string PersonEmail
    {
        get => _personEmail;
        set => _personEmail = value;
    }
    
    
    public DateTime DateStart
    {
        get => _dateStart;
        set
        {
            _dateStart = value;
            IfHasInvalidDateThrowException();
        }
    }

    public DateTime DateEnd
    {
        get => _dateEnd;
        set
        {
            _dateEnd = value;
            IfHasInvalidDateThrowException();
        }
    }
    
    public StorageUnit StorageUnit
    {
        get => _storageUnit;
        set => _storageUnit = value;
    }
    
    
    public string RejectedMessage
    {

        get => _rejectedMessage;
        set
        {
            _rejectedMessage = value;
            IfHasInvalidRejectionThrowException();
        }
    }

    public string Status
    {
        get => _status;
        set => _status = value;

    }
    
    public bool Payment
    {

        get => _payment;
        set => _payment = value;

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
        double totalPrice = _storageUnit.CalculateStorageUnitPricePerDay() * GetCountOfDays();
        return CheckDiscount(totalPrice);
    }

    private double CheckDiscount(double totalPrice)
    {
        double discount;
        double totalPriceWithDiscount;
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
        int maxRejectionLength = 300;
        return RejectedMessage.Length <= maxRejectionLength;
    }

    public bool CheckDate()
    {
        return _dateStart < _dateEnd;
    }
    
    private void IfHasInvalidDateThrowException()
    {
        if (!CheckDate())
        {
            throw new BookingExceptions("Date is not valid");
        }
    }
}
