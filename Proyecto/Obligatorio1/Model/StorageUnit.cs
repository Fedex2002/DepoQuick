using Model.Enums;

namespace Model;

public class StorageUnit
{
    private AreaType _area;
    private SizeType _size;
    private bool _climatization;
    private List<Promotion>? _promotions;
    public StorageUnit()
    {
    }
    
    public StorageUnit(AreaType area, SizeType size, bool climatization, List<Promotion> promotions)
    {
        this._area = area;
        this._size = size;
        this._climatization = climatization; 
        this._promotions = promotions;
    }
       
    public AreaType GetArea()
    {
        return _area;
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
    
    public int CalculateStorageUnitPrice()
    {
        return SizeOfStorageUnit();
    }
    private int SizeOfStorageUnit()
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
    
    
}