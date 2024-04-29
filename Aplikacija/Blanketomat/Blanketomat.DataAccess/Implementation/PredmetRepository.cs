using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;

namespace Blanketomat.DataAccess.Implementation;

public class PredmetRepository : GenericRepository<Predmet>, IPredmetRepository
{
    public PredmetRepository(BlanketomatContext context) : base(context)
    {
        
    }
}