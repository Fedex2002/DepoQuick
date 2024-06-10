using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Exceptions;
namespace Model;

public class Promotion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    private string _label = "";
    private int _discount;
    private DateTime _dateStart = DateTime.MinValue;
    private DateTime _dateEnd = DateTime.MaxValue;
    public List<StorageUnit> StorageUnits { get; set; } = new List<StorageUnit>();
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
        int maxLabelLength = 20;
        return Label.Length <= maxLabelLength;
    }

    public bool ValidateDiscount()
    {
        int minDiscount = 5;
        int maxDiscount = 75;
        return _discount >= minDiscount && _discount <= maxDiscount;
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