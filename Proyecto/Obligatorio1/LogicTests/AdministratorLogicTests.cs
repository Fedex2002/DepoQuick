using Repositories;
using Logic;
using Logic.DTOs;
using Model.Enums;

namespace LogicTests;

[TestClass]
public class AdministratorLogicTests
{
    private PersonRepositories _personRepo;
    private AdministratorLogic _administratorLogic;
    private AdministratorDto _administratorDto;
    private BookingDto _bookingDto;

    [TestInitialize]
    public void TestInitialize()
    {
        _personRepo = new PersonRepositories();
        _administratorLogic = new AdministratorLogic(_personRepo);
        _administratorDto = new AdministratorDto("Franco", "Ramos", "francoramos1511@gmail.com", "PassWord921#");
    }
    
    [TestMethod]
    public void WhenAdministratorApprovesABookingDtoShouldChangeItToTrue()
    {
        _bookingDto = new BookingDto(false, new DateTime(2023, 7, 5), new DateTime(2026, 8, 15), new StorageUnitDto("12", AreaType.A, SizeType.Small, true, new List<PromotionDto>()), "");
        _bookingDto = _administratorLogic.ApproveBooking(_bookingDto);
        Assert.AreEqual(true, _bookingDto.Approved);
    }
}