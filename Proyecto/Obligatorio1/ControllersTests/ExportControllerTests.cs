using DataAccess.Context;
using DataAccess.Repository;
using Logic;

namespace LogicTests;

[TestClass]

public class ExportControllerTests
{
    private ApplicationDbContext _context;
    private ExportController _exportController;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private BookingsRepository _personRepo;

    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _personRepo = new BookingsRepository(_context);
        _exportController = new ExportController(_context);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void WhenExportingAsCsvShouldExportIt()
    {
       _exportController.Export("csv");
       
    }

}