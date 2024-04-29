using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;

namespace Blanketomat.DataAccess.Implementation;

public class AkreditacijaRepository : GenericRepository<Akreditacija>, IAkreditacijaRepository
{
    public AkreditacijaRepository(BlanketomatContext context) : base(context)
    {
        
    }
}