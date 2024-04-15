using Model;

namespace ModelTests;

[TestClass]
public class UserTests
{
    private User _myuser;

    [TestInitialize]
    public void TestInitialize()
    {
        _myuser = new User();
    }

    [TestMethod]
    public void CreatingEmptyUserShouldReturnEmpty()
    {
        Assert.IsNotNull(_myuser);
    }

    [TestMethod]
    public void CreatingUserWithValidations_ShouldReturnValues()
    {
        _myuser = new User(string name, string surname, string email, string password, Booking _mybooking);
        Assert.AreEqual(name, _myuser.GetName());
        Assert.AreEqual(surname, _myuser.GetSurname());
        Assert.AreEqual(email, _myuser.GetEmail());
        Assert.AreEqual(password, _myuser.GetPassword());
        Assert.AreEqual(_mybooking, _myuser.GetBooking());
    }
}