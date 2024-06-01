using DataAccess.Context;
using DataAccess.Repository;
using Model;
using Model.Exceptions;

namespace DataAccessTests;

[TestClass]
public class PromotionRepositoryTests
{
    private PromotionsRepository _repository;
    private ApplicationDbContext _context;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private Promotion _myPromotion;
    
    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _repository = new PromotionsRepository(_context);
        DateTime dateStart = new DateTime(2024,7,15);
        DateTime dateEnd = new DateTime(2024,10,15);
        _myPromotion = new Promotion("Descuento Invierno", 25, dateStart, dateEnd);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void WhenAddingANewPromotion_ShouldAddTheNewPromotionInPromotionsTable()
    {
        _repository.AddPromotion(_myPromotion);

        var promotionInDb = _context.Promotions.First();
        
        Assert.AreEqual(_myPromotion, promotionInDb);
    }
    
    [TestMethod]
    [ExpectedException(typeof(RepositoryExceptions))]
    public void WhenAddingAPromotionThatAlreadyExists_ShouldThrowAnException()
    {
        _repository.AddPromotion(_myPromotion);
        _repository.AddPromotion(_myPromotion);
    }
}