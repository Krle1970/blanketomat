using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;

namespace Blanketomat.DataAccess.Implementation;

public class AsistentRepository : GenericRepository<Asistent>, IAsistentRepository
{
    public AsistentRepository(BlanketomatContext context) : base(context)
    {
        
    }
}