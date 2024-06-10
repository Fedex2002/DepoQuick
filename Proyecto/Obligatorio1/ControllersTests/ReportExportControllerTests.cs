using Controllers;
using DataAccess.Context;
using DataAccess.Repository;

namespace LogicTests;

[TestClass]

public class ReportExportControllerTests
{
    private ApplicationDbContext _context;
    private ReportExportController _exportController;
    private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    private BookingsRepository _bookingsRepo;

    [TestInitialize]
    public void TestInitialize()
    {
        _context = _contextFactory.CreateDbContext();
        _bookingsRepo = new BookingsRepository(_context);
        _exportController = new ReportExportController(_context);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void WhenExportingAsCsvShouldExportIt()
    {
       string expectedData = _exportController.Export("csv");
       Assert.IsNotNull(expectedData);
    }
    
    [TestMethod]
    public void WhenExportingAsTxtShouldExportIt()
    {
       string expectedData = _exportController.Export("txt");
       Assert.IsNotNull(expectedData);
    }
    

}