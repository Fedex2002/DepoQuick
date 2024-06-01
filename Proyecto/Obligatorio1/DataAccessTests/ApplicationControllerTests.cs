using DataAccess.Context;


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
            Assert.IsNotNull(_controller.Context);
        }
    }
}