using Controllers;
using Logic;

namespace ControllerTests;

[TestClass]
public class PersonControllerTests
{
    private PersonController _personController;
    private PersonLogic _personLogic;
    [TestMethod]
    public void WhenCreatingAPersonControllerCantBeNull()
    {
        _personController = new PersonController(_personLogic);
        Assert.IsNotNull(_personController);
    }
}