using DataAccess.Context;
using Logic;
using Logic.DTOs;
using Model;


namespace DataAccessTests
{
    [TestClass]
    public class ApplicationControllerTests
    {
        private ApplicationDbContext _context;
        private ApplicationController _controller;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();

        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _controller = new ApplicationController(_context);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenControllerIsCreated_ThenContextIsNotNull()
        {
            Assert.IsNotNull(_controller);
        }
        
        [TestMethod]
        public void WhenControllerReceivesAPromotionDto_ShouldMapToPromotionObject()
        {
            PromotionDto promotionDto = new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            
            Promotion promotion = _controller.CreatePromotion(promotionDto);
            
            Assert.IsInstanceOfType(promotion, typeof(Promotion));
        }
        
        [TestMethod]
        public void WhenControllerAddsNewPromotion_ShouldAddItToPromotionsRepository()
        {
            PromotionDto promotionDto = new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            
            Promotion promotion = _controller.CreatePromotion(promotionDto);
            
            _controller.AddPromotion(promotion);
            
            Assert.AreEqual(1, _controller.PromotionsRepository.GetAllPromotions().Count);
        }

        [TestMethod]
        public void WhenControllerModifiesThePromotion_ShouldReturnTheNewPromotion()
        {
            PromotionDto promotionDto = new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            Promotion promotion = _controller.CreatePromotion(promotionDto);
            _controller.AddPromotion(promotion);
            
            PromotionDto newPromotionDto = new PromotionDto("Summer discount", 50, new DateTime(2025, 7, 15), new DateTime(2025, 10, 15));
            
            _controller.UpdatePromotion(promotion, newPromotionDto);
            
            Assert.AreEqual(promotion, _controller.PromotionsRepository.FindPromotionByLabel(promotion.Label));
        }
    }
}