using DataAccess.Context;
using DataAccess.Repository;

namespace Logic;

public class ApplicationController
{
    public PromotionsRepository PromotionsRepository;
    
    public ApplicationController(ApplicationDbContext context)
    {
        PromotionsRepository = new PromotionsRepository(context);
    }
}