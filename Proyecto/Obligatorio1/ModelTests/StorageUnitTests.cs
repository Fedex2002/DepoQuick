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
    public void CalculatingDepositPriceWithValidations_ShouldReturnPrice()
    {
        AreaType area = AreaType.A;
        SizeType size = SizeType.Small;
        List<Promotion> p = new List<Promotion>();
        StorageUnit storageUnit = new StorageUnit(area, size, true, p);
        Assert.AreEqual(100, storageUnit.CalculateDepositPrice());
    }
}