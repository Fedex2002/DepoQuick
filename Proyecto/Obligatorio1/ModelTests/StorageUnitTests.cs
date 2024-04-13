using System.Transactions;
using Model;
using Model.Enums;
namespace ModelTests;

[TestClass]
public class StorageUnitTests
{
    private StorageUnit _mystorageunit;
    private List<Promotion> _promotions;
    private Promotion _mypromotion;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _promotions = new List<Promotion>();
       _mypromotion = new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        _promotions.Add(_mypromotion);
        _mystorageunit = new StorageUnit();
    }

    [TestMethod]
    public void CreatingEmptyStorageUnitShouldReturnEmpty()
    {
        
        Assert.IsNotNull(_mystorageunit);
    }
    
    [TestMethod]
    public void CreatingAStorageUnitWithValues_ShouldReturnValues()
    {
        AreaType area = AreaType.A;
        SizeType size = SizeType.Small;
        List<Promotion> p = new List<Promotion>();
        StorageUnit storageUnit = new StorageUnit(area, size, true, p);
        Assert.AreEqual(AreaType.A, storageUnit.GetArea());
        Assert.AreEqual(SizeType.Small, storageUnit.GetSize());
        Assert.AreEqual(true, storageUnit.GetClimatization());
        Assert.AreEqual(p, storageUnit.GetPromotions());
    }
 
    [TestMethod]
    public void CalculatingStorageUnitPricePerDayWithValidations_ShouldReturnPrice()
    {
        Assert.AreEqual(0, _mystorageunit.CalculateStorageUnitPrice());
        
        _mystorageunit= new StorageUnit(AreaType.A, SizeType.Small, true,_promotions );
        Assert.AreEqual(52.5, _mystorageunit.CalculateStorageUnitPrice());
        
        _mystorageunit = new StorageUnit(AreaType.B, SizeType.Medium, false, null);
        Assert.AreEqual(75, _mystorageunit.CalculateStorageUnitPrice());
        
        _mystorageunit = new StorageUnit(AreaType.C, SizeType.Large, true, null);
        Assert.AreEqual(120, _mystorageunit.CalculateStorageUnitPrice());
    }
}