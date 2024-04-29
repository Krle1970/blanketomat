using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanketomat.DataAccess.Implementation;

public class SmerRepository : GenericRepository<Smer>, ISmerRepository
{
    public SmerRepository(BlanketomatContext context) : base(context)
    {
        
    }
}