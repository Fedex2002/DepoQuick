using System.Runtime.InteropServices.ComTypes;
using Repositories;
using Model;
using Logic;
using Logic.DTOs;
using Model.Exceptions;

namespace LogicTests;

[TestClass]
public class PromotionLogicTests
{
    private PromotionsRepositories _promotionRepo;
    private PromotionLogic _promotionLogic;
    private Promotion _promotion;
    private PromotionDto _promotionDto;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _promotionRepo = new PromotionsRepositories();
        _promotionLogic = new PromotionLogic(_promotionRepo);
        _promotion = new Promotion("Winter discount", 25, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
        _promotionDto= new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
    }

    [TestMethod]
    public void WhenEmptyPromotionDtoIsCreatedShouldReturnEmptyPromotionDto()
    {
        PromotionDto promotionDto = new PromotionDto();
        Assert.IsNotNull(promotionDto);
    }    
    
    [TestMethod] 
    public void WhenModifyingPromotionShouldEliminateTheOldOneAndAddTheNewOne()
    {
        _promotionRepo.AddToRepository(_promotion);
        _promotionDto= new PromotionDto("Winter discount", 50, new DateTime(2025, 7, 15), new DateTime(2025, 10, 15));
        _promotionLogic.ModifyPromotion(_promotionDto);
        Assert.AreEqual(_promotionDto.Label, _promotionRepo.GetFromRepository(_promotionDto.Label).GetLabel());
        Assert.AreEqual(_promotionDto.Discount, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDiscount());
        Assert.AreEqual(_promotionDto.DateStart, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDateStart());
        Assert.AreEqual(_promotionDto.DateEnd, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDateEnd());
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToModifyANonExistingPromotionShouldThrowException()
    {
        _promotionLogic.ModifyPromotion(_promotionDto);
    }
    
    [TestMethod]
    public void WhenPromotionIsCreatedShouldBeAddedToRepository()
    {
        _promotionLogic.CreatePromotion(_promotionDto);
        Assert.AreEqual(_promotionDto.Label, _promotionRepo.GetFromRepository(_promotionDto.Label).GetLabel());
        Assert.AreEqual(_promotionDto.Discount, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDiscount());
        Assert.AreEqual(_promotionDto.DateStart, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDateStart());
        Assert.AreEqual(_promotionDto.DateEnd, _promotionRepo.GetFromRepository(_promotionDto.Label).GetDateEnd());
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenPromotionIsCreatedWithAnExistingLabelShouldThrowException()
    {
        _promotionRepo.AddToRepository(_promotion);
        _promotionLogic.CreatePromotion(_promotionDto);
    }
    
    [TestMethod]
    public void WhenPromotionIsEliminatedShouldBeRemovedFromRepository()
    {
        _promotionRepo.AddToRepository(_promotion);
        _promotionLogic.RemovePromotion(_promotionDto);
        Assert.IsNull(_promotionRepo.GetFromRepository(_promotion.GetLabel()));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicExceptions))]
    public void WhenTryingToEliminateANonExistingPromotionShouldThrowException()
    {
        _promotionLogic.RemovePromotion(_promotionDto);
    }
    
    [TestMethod]
    public void WhenGettingPromotionsDtoShouldReturnAListOfPromotionsDto()
    {
        _promotionRepo.AddToRepository(_promotion);
        List<PromotionDto> promotionsDto = _promotionLogic.GetPromotionsDto();
        Assert.AreEqual(_promotion.GetLabel(), promotionsDto[0].Label);
        Assert.AreEqual(_promotion.GetDiscount(), promotionsDto[0].Discount);
        Assert.AreEqual(_promotion.GetDateStart(), promotionsDto[0].DateStart);
        Assert.AreEqual(_promotion.GetDateEnd(), promotionsDto[0].DateEnd);
    }
}