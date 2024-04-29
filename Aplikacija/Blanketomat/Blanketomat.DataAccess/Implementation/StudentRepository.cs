using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Models;
using Blanketomat.Domain.Repository;

namespace Blanketomat.DataAccess.Implementation;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(BlanketomatContext context) : base(context)
    {
        
    }
}