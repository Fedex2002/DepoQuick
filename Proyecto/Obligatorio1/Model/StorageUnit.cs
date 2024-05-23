using Model.Enums;

namespace Model;

public class StorageUnit
{
    private  string _id;
    private  AreaType _area;
    private  SizeType _size;
    private  bool _climatization;
    private  List<Promotion>? _promotions;
    public StorageUnit()
    {
    }
    
    public StorageUnit(string id,AreaType area, SizeType size, bool climatization, List<Promotion> promotions)
    {
        Id = id;
        Area = area;
        Size = size;
       Climatization = climatization; 
       Promotions = promotions;
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
        int c = 0;
        if (_climatization)
        {
            c = 20;
        }
        return c;
    }
    
    private double GetValuePromotions()
    {
        int p = 0;
        if (_promotions != null)
        {
            foreach (Promotion promotion in _promotions)
            {
                p += promotion.Discount;
            }
        }
        return p;
    }
}