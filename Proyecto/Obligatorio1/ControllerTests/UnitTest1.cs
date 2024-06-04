namespace ControllerTests;

[TestClass]
public class UnitTest1
{
    private PersonController _personController;
    private PersonLogic _personLogic;
    [TestMethod]
    public void WhenCreatingAPersonControllerCantBeNull()
    {
        _personController = new PersonController();
        Assert.IsNotNull(_personController);
    }
}