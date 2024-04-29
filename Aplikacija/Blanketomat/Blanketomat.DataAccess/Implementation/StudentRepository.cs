using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanketomat.DataAccess.Implementation;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(BlanketomatContext context) : base(context)
    {
        
    }
}