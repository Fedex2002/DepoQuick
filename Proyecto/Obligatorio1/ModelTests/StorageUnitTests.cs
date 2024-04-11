using System.Transactions;
using Model;
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
        Promotion p = new Promotion();
        StorageUnit storageUnit = new StorageUnit(area, size, true, p);
        Assert.AreEqual(AreaType.A, storageUnit.GetArea());
        Assert.AreEqual(SizeType.Small, storageUnit.GetSize());
        Assert.AreEqual(true, storageUnit.GetClimatization());
        Assert.AreEqual(p, storageUnit.GetPromotion());
    }
}