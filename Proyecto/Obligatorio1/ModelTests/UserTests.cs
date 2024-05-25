using Model;

namespace ModelTests;

[TestClass]
public class UserTests
{
    private User _myuser;
    private List<Booking> _bookings;

    [TestInitialize]
    public void TestInitialize()
    {
        _bookings = new List<Booking>();
        _myuser = new User("Franco", "Ramos", "francoramos1511@gmail.com", "FrancoRamos2023#", _bookings);
    }

    [TestMethod]
    public void CreatingEmptyUserShouldReturnEmpty()
    {
        _myuser = new User();
        Assert.IsNotNull(_myuser);
    }

    [TestMethod]
    public void CreatingUserWithValidations_ShouldReturnValues()
    {
        Assert.AreEqual("Franco", _myuser.Name);
        Assert.AreEqual("Ramos", _myuser.Surname);
        Assert.AreEqual("francoramos1511@gmail.com", _myuser.Email);
        Assert.AreEqual("FrancoRamos2023#", _myuser.Password);
        Assert.AreEqual(_bookings, _myuser.Bookings);
    }
}