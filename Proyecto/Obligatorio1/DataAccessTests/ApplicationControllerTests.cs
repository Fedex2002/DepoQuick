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
            Promotion newPromotion = _controller.CreatePromotion(newPromotionDto);
            
            _controller.UpdatePromotion(promotion.Label, newPromotion);
            
            Assert.AreEqual(promotion, _controller.PromotionsRepository.FindPromotionByLabel(promotion.Label));
        }

        [TestMethod]
        public void WhenControllerRemovesThePromotion_ShouldDeleteThePromotion()
        {
            PromotionDto promotionDto = new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            Promotion promotion = _controller.CreatePromotion(promotionDto);
            _controller.AddPromotion(promotion);
            
            Promotion promotionToDelete = _controller.PromotionsRepository.FindPromotionByLabel(promotionDto.Label);
            
            _controller.DeletePromotion(promotionToDelete);
            
            Assert.IsFalse(_controller.PromotionsRepository.PromotionAlreadyExists(promotion.Label));
        }

        [TestMethod]
        public void WhenControllerNeedsToGetAllPromotions_ShouldReturnAListOfPromotionsDto()
        {
            PromotionDto promotionDto = new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            Promotion promotion = _controller.CreatePromotion(promotionDto);
            _controller.AddPromotion(promotion);
            
            List<PromotionDto> promotions = _controller.GetPromotionsDto();
            
            Assert.AreEqual(1, promotions.Count);
        }
        
        [TestMethod]
        public void WhenControllerNeedsToGetAPromotionDtoByLabel_ShouldReturnThePromotionDto()
        {
            PromotionDto promotionDto = new PromotionDto("Winter discount", 30, new DateTime(2024, 7, 15), new DateTime(2024, 10, 15));
            Promotion promotion = _controller.CreatePromotion(promotionDto);
            _controller.AddPromotion(promotion);
            
            PromotionDto promotionDtoToFind = _controller.GetPromotionDtoFromLabel(promotionDto.Label);
            
            Assert.AreEqual(promotionDto.Label, promotionDtoToFind.Label);
        }

        [TestMethod]

        public void WhenControlllerCreatesAPersonShouldReturnAPersonObject()
        {
            PersonDto personDto = new PersonDto("Fede", "Ramos", "FedeRamos@gmail.com", "PaSSWorD921#",false);
            _controller.CreatePerson(personDto);
            Assert.IsInstanceOfType(_controller.CreatePerson(personDto), typeof(Person));
            
        }

        [TestMethod]

        public void WhenControllerAddsAPersonShouldAddItToTheRepository()
        {
            Person person = new Person("Fede", "Ramos", "FedeRamos@gmail.com", "PaSSWorD921#",false);
            _controller.AddPerson(person);
            Assert.AreEqual(1, _controller.PersonsRepository.GetAllPersons().Count);
        }

        [TestMethod]

        public void WhenControllerGetsAllPersonsDtoShouldReturnAListOfPersonsDto()
        {
            Person person = new Person("Fede", "Ramos", "FedeRamos@gmail.com", "PaSSWorD921#",false);
            _controller.AddPerson(person);
            List<PersonDto> persons = _controller.GetPersonsDto();
            Assert.AreEqual(1, persons.Count);
        }
    }
}