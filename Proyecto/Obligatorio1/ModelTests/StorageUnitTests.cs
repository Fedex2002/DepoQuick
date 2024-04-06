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
}