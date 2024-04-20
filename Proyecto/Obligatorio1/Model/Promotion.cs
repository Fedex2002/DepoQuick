using Model.Exceptions;
namespace Model;

public class Promotion
{
    private string _label;
    private int _discount;
    private DateTime _dateStart;
    private DateTime _dateEnd;
    public Promotion()
    {
        
    }
    public Promotion(string label, int discount, DateTime dateStart, DateTime dateEnd)
    {
        _label = "";
        SetLabel(label);
        _discount = discount;
        _dateStart = dateStart;
        _dateEnd = dateEnd;
    }

    public bool ValidateLabel()
    {
        return _label.Length <= 20;
    }
    
    public bool ValidateDiscount()
    {
        return _discount >= 5 && _discount <= 75;
    }
    
    public bool ValidateDate()
    {
        return _dateStart < _dateEnd;
    }
    
    public string GetLabel()
    {
        return _label;
    }
    
    public int GetDiscount()
    {
        return _discount;
    }
    
    public DateTime GetDateStart()
    {
        return _dateStart;
    }
    
    public DateTime GetDateEnd()
    {
        return _dateEnd;
    }

    private void SetLabel(string label)
    {
        _label = label;
        if (!ValidateLabel())
        {
            throw new PromotionExceptions("Label is not valid (max 20 characters)");
        }
    }
}