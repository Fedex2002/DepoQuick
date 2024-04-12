using System.Transactions;
using Model;
using Model.Enums;
namespace ModelTests;

[TestClass]
public class StorageUnitTests
{
    private StorageUnit _mystorageunit;

    [TestInitialize]
    public void TestInitialize()
    {
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
    public void CalculatingStorageUnitPriceWithValidations_ShouldReturnPrice()
    {
        List<Promotion> p = new List<Promotion>();
        Promotion myPromotion = new Promotion("Descuento Invierno", 25, new DateTime(2024,7,15), new DateTime(2024,10,15));
        p.Add(myPromotion);
        
        StorageUnit storageUnit = new StorageUnit();
        Assert.AreEqual(0, storageUnit.CalculateStorageUnitPrice());
        
        StorageUnit storageUnitSmall = new StorageUnit(AreaType.A, SizeType.Small, true, p);
        Assert.AreEqual(52.5, storageUnitSmall.CalculateStorageUnitPrice());
        
        StorageUnit storageUnitMedium = new StorageUnit(AreaType.B, SizeType.Medium, false, null);
        Assert.AreEqual(75, storageUnitMedium.CalculateStorageUnitPrice());
        
        StorageUnit storageUnitLarge = new StorageUnit(AreaType.C, SizeType.Large, true, null);
        Assert.AreEqual(120, storageUnitLarge.CalculateStorageUnitPrice());
    }
}