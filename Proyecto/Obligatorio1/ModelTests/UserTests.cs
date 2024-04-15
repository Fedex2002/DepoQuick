using Model;

namespace ModelTests;

[TestClass]
public class UserTests
{
    private User _myuser;
    private Booking _booking;

    [TestInitialize]
    public void TestInitialize()
    {
        _booking = new Booking();
        _myuser = new User("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#", _booking);
    }

    [TestMethod]
    public void CreatingEmptyUserShouldReturnEmpty()
    {
        Assert.IsNotNull(_myuser);
    }

    [TestMethod]
    public void CreatingUserWithValidations_ShouldReturnValues()
    {
        Assert.AreEqual("Franco", _myuser.GetName());
        Assert.AreEqual("Ramos", _myuser.GetSurname());
        Assert.AreEqual("francoramos1511@gmail.com", _myuser.GetEmail());
        Assert.AreEqual("FrancoRamos2023#", _myuser.GetPassword());
        Assert.AreEqual(_booking, _myuser.GetBooking());
    }
}