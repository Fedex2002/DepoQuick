using Model.Exceptions;
namespace Model;

public class Promotion
{
    private string _label = "";
    private int _discount;
    private DateTime _dateStart = DateTime.MinValue;
    private DateTime _dateEnd = DateTime.MaxValue;
    public Promotion()
    {
        
    }
    public Promotion(string label, int discount, DateTime dateStart, DateTime dateEnd)
    {
        Label = label;
        Discount = discount;
        DateStart = dateStart;
        DateEnd = dateEnd;
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
    
    public DateTime DateStart
    {
        get => _dateStart;
        set => _dateStart = value;
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
    

    public int Discount
    {
        get => _discount;
        set
        {
            _discount = value;
            IfHasInvalidDiscountThrowException();
        }
    }

    
    public bool ValidateLabel()
    {
        return Label.Length <= 20;
    }

    public bool ValidateDiscount()
    {
        return _discount >= 5 && _discount <= 75;
    }
    
    public bool ValidateDate()
    {
        return _dateStart < _dateEnd;
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
    


    private void IfHasInvalidDateThrowException()
    {
        if (!ValidateDate())
        {
            throw new PromotionExceptions("Date is not valid (start date must be before end date)");
        }
    }
}