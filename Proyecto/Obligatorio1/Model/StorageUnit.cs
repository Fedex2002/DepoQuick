using Model.Enums;
using Model.Exceptions;

namespace Model;

public class StorageUnit
{
    private  string _id;
    private  AreaType _area;
    private  SizeType _size;
    private  bool _climatization;
    private  List<Promotion>? _promotions;
    private List<DateRange> _availableDates;
    public StorageUnit()
    {
        
    }
    
    public StorageUnit(string id,AreaType area, SizeType size, bool climatization, List<Promotion> promotions, List<DateRange> availableDates)
    {
        Id = id;
        Area = area;
        Size = size;
        Climatization = climatization;
        checkIfPromotionsDoesntExceedOneHundred(promotions);
        Promotions = promotions;
        AvailableDates = availableDates;
    }

    private void checkIfPromotionsDoesntExceedOneHundred(List<Promotion> promotions)
    {
        if (promotions != null)
        {
            int totalDiscount = 0;
            foreach (Promotion promotion in promotions)
            {
                totalDiscount += promotion.Discount;
            }
            if (totalDiscount >= 100)
            {
                throw new StorageUnitExceptions("The total discount of the promotions exceeds or are equal to 100%");
            }
        }
    }

    public string Id
    {
        get => _id;
        set => _id = value;
    }
       
    public AreaType Area
    {
        get => _area;
        set => _area = value;
    }
    
    public SizeType Size
    {
        get => _size;
        set => _size = value;
    }
    
    public bool Climatization
    {
        get => _climatization;
        set => _climatization = value;
    }
    public List<Promotion>? Promotions
    {
        get => _promotions;
        set => _promotions = value;
    }
    
    public List<DateRange> AvailableDates
    {
        get => _availableDates;
        set => _availableDates = value;
    }

    
    public double CalculateStorageUnitPricePerDay()
    {
        double price = ValueOfSizeOfStorageUnit() + ValueOfClimatization();
        if (HasPromotions())
        {
            price -= RuleOf3();
        }
        
        return price;
    }
    
    private double RuleOf3()
    {
        return ((ValueOfSizeOfStorageUnit() + ValueOfClimatization()) * GetValuePromotions()) / 100;
    }
    
    private bool HasPromotions()
    {
        return _promotions != null;
    }
    private double ValueOfSizeOfStorageUnit()
    {
        int size = 0;
        if (_size == SizeType.Small)
        {
            size = 50;
        } else if (_size == SizeType.Medium)
        {
            size = 75;
        } else if (_size == SizeType.Large)
        {
            size = 100;
        }
        return size;
    }
    
    private double ValueOfClimatization()
    {
        int valueOfClimatization = 0;
        if (_climatization)
        {
            valueOfClimatization = 20;
        }
        return valueOfClimatization;
    }
    
    private double GetValuePromotions()
    {
        int promotionDiscount = 0;
        if (_promotions != null)
        {
            foreach (Promotion promotion in _promotions)
            {
                promotionDiscount += promotion.Discount;
            }
        }
        return promotionDiscount;
    }
    
    public void AddDateRange(DateRange dateRange)
    {
        _availableDates.Add(dateRange);
    }
    
    public bool IsInDateRange(DateTime date)
    {
        return _availableDates.Any(range => range.Includes(date));
    }
}

