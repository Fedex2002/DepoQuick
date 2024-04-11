using Model.Enums;

namespace Model;

public class StorageUnit
{
    private AreaType area = AreaType.A;
    private SizeType size = SizeType.Small;
    private bool climatization = false;
    private Promotion? promotion = new Promotion();
    public StorageUnit()
    {
    }
    
    public StorageUnit(AreaType area, SizeType size, bool climatization, Promotion promotion)
    {
        this.area = area;
        this.size = size;
        this.climatization = climatization;
        this.promotion = promotion;
    }
    
    public AreaType GetArea()
    {
        return area;
    }
    
    public SizeType GetSize()
    {
        return size;
    }
    
    public bool GetClimatization()
    {
        return climatization;
    }
    
    public Promotion GetPromotion()
    {
        return promotion;
    }
}