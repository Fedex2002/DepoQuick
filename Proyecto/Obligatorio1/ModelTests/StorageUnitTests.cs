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
        string id = "id";
        _mystorageunit = new StorageUnit(id,area, size, true, _promotions);
        Assert.AreEqual(AreaType.A, _mystorageunit.Area);
        Assert.AreEqual(SizeType.Small, _mystorageunit.Size);
        Assert.AreEqual(true, _mystorageunit.Climatization);
        Assert.AreEqual(_promotions, _mystorageunit.GetPromotions());
        Assert.AreEqual(id,_mystorageunit.Id);
    }
 
    [TestMethod]
    public void CalculatingStorageUnitPricePerDayWithValidations_ShouldReturnPrice()
    {
        Assert.AreEqual(0, _mystorageunit.CalculateStorageUnitPricePerDay());
        
        _mystorageunit= new StorageUnit("",AreaType.A, SizeType.Small, true,_promotions );
        Assert.AreEqual(52.5, _mystorageunit.CalculateStorageUnitPricePerDay());

        _promotions = new List<Promotion>();
        
        _mystorageunit = new StorageUnit("",AreaType.B, SizeType.Medium, false, _promotions);
        Assert.AreEqual(75, _mystorageunit.CalculateStorageUnitPricePerDay());
        
        _mystorageunit = new StorageUnit("",AreaType.C, SizeType.Large, true, _promotions);
        Assert.AreEqual(120, _mystorageunit.CalculateStorageUnitPricePerDay());
    }
}