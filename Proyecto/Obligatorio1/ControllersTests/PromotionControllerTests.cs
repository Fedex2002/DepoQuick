using DataAccess.Context;
using DataAccess.Repository;
using Logic;
using Logic.DTOs;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class PromotionControllerTests
{
    private ApplicationDbContext _context;
    private PromotionController _promotionController;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private PromotionsRepository _promotionRepo;
    private PromotionDto _promotionDto;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _promotionController = new PromotionController(_context);
        _promotionRepo = new PromotionsRepository(_context);
        _promotionDto= new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void WhenEmptyPromotionDtoIsCreatedShouldReturnEmptyPromotionDto()
    {
        PromotionDto promotionDto = new PromotionDto();
        Assert.IsNotNull(promotionDto);
    }    
    
    [TestMethod] 
    public void WhenModifyingPromotionShouldModifyTheExistingPromotion()
    {
        _promotionController.CreatePromotion(_promotionDto);
        _promotionDto= new PromotionDto("Summer discount", 50, new DateTime(2025, 7, 15), new DateTime(2025, 10, 15));
        _promotionController.ModifyPromotion("Winter discount", _promotionDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToModifyANonExistingPromotionShouldThrowException()
    {
        _promotionController.ModifyPromotion("Winter discount", _promotionDto);
    }
    
    [TestMethod]
    public void WhenPromotionIsCreatedShouldBeAddedToRepository()
    {
        _promotionController.CreatePromotion(_promotionDto);
        Assert.AreEqual(_promotionDto.Label, _promotionController.GetPromotionDtoFromLabel(_promotionDto.Label).Label);
        Assert.AreEqual(_promotionDto.Discount, _promotionController.GetPromotionDtoFromLabel(_promotionDto.Label).Discount);
        Assert.AreEqual(_promotionDto.DateStart, _promotionController.GetPromotionDtoFromLabel(_promotionDto.Label).DateStart);
        Assert.AreEqual(_promotionDto.DateEnd, _promotionController.GetPromotionDtoFromLabel(_promotionDto.Label).DateEnd);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPromotionIsCreatedWithAnExistingLabelShouldThrowException()
    {
        _promotionController.CreatePromotion(_promotionDto);
        _promotionController.CreatePromotion(_promotionDto);
    }
    
    [TestMethod]
    public void WhenPromotionIsEliminatedShouldBeRemovedFromRepository()
    {
        _promotionController.CreatePromotion(_promotionDto);
        _promotionController.RemovePromotion(_promotionDto);
        Assert.IsNull(_promotionRepo.FindPromotionByLabel(_promotionDto.Label));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToEliminateANonExistingPromotionShouldThrowException()
    {
        _promotionController.RemovePromotion(_promotionDto);
    }
    
    [TestMethod]
    public void WhenGettingPromotionsDtoShouldReturnAListOfPromotionsDto()
    {
        _promotionController.CreatePromotion(_promotionDto);
        List<PromotionDto> promotionsDto = _promotionController.GetPromotionsDto();
        Assert.IsNotNull(promotionsDto);
    }

    [TestMethod]
    public void WhenGettingPromotionsDtoFromLabelShouldReturnIt()
    {
        _promotionController.CreatePromotion(_promotionDto);
        PromotionDto promotionDto = _promotionController.GetPromotionDtoFromLabel(_promotionDto.Label);
        Assert.AreEqual(_promotionDto.Label, promotionDto.Label);
    }
}