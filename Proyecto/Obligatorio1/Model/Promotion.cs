using Model.Exceptions;
namespace Model;

public class Promotion
{
    private string _label = "";
    private int _discount;
    private DateTime _dateStart;
    private DateTime _dateEnd;
    public Promotion()
    {
        
    }
    public Promotion(string label, int discount, DateTime dateStart, DateTime dateEnd)
    {
        Label = label;
        Discount = discount;
        _dateStart = DateTime.MinValue;
        _dateEnd = DateTime.MaxValue;
        SetDate(dateStart, dateEnd);
    }

    public bool ValidateLabel()
    {
        return Label.Length <= 20;
    }
    
    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            IfHasInvalidLabelThrowException();
        }
    }

    public int Discount
    {
        get => _discount;
        set
        {
            _discount = value;
            IfHasInvalidDiscountThrowException();
        }
    }


    public bool ValidateDiscount()
    {
        return _discount >= 5 && _discount <= 75;
    }
    
    public bool ValidateDate()
    {
        return _dateStart < _dateEnd;
    }
    

    
    public DateTime GetDateStart()
    {
        return _dateStart;
    }
    
    public DateTime GetDateEnd()
    {
        return _dateEnd;
    }
    

    private void IfHasInvalidLabelThrowException()
    {
        if (!ValidateLabel())
        {
            throw new PromotionExceptions("Label is not valid (max 20 characters)");
        }
    }
    


    private void IfHasInvalidDiscountThrowException()
    {
        if (!ValidateDiscount())
        {
            throw new PromotionExceptions("Discount is not valid (between 5 and 75)");
        }
    }
    
    private void SetDate(DateTime dateStart, DateTime dateEnd)
    {
        _dateStart = dateStart;
        _dateEnd = dateEnd;
        IfHasInvalidDateThrowException();
    }

    private void IfHasInvalidDateThrowException()
    {
        if (!ValidateDate())
        {
            throw new PromotionExceptions("Date is not valid (start date must be before end date)");
        }
    }
}