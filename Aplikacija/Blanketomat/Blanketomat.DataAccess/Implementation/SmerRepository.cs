using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;

namespace Blanketomat.DataAccess.Implementation;

public class SmerRepository : GenericRepository<Smer>, ISmerRepository
{
    public SmerRepository(BlanketomatContext context) : base(context)
    {
        
    }
}