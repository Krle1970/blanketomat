using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;

namespace Blanketomat.DataAccess.Implementation;

public class ProfesorRepository : GenericRepository<Profesor>, IProfesorRepository
{
    public ProfesorRepository(BlanketomatContext context) : base(context)
    {
        
    }
}