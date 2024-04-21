using Model.Enums;

namespace Model;

public class StorageUnit
{
    private int _id;
    private AreaType _area;
    private SizeType _size;
    private bool _climatization;
    private List<Promotion>? _promotions;
    public StorageUnit()
    {
    }
    
    public StorageUnit(int id,AreaType area, SizeType size, bool climatization, List<Promotion> promotions)
    {
        this._id = id;
        this._area = area;
        this._size = size;
        this._climatization = climatization; 
        this._promotions = promotions;
    }
       
    public AreaType GetArea()
    {
        return _area;
    }
    
    public int GetId()
    {
        return _id;
    }
    
    public SizeType GetSize()
    {
        return _size;
    }
    
    public bool GetClimatization()
    {
        return _climatization;
    }
    
    public List<Promotion> GetPromotions()
    {
        return _promotions;
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
                p += promotion.GetDiscount();
            }
        }
        return p;
    }
}