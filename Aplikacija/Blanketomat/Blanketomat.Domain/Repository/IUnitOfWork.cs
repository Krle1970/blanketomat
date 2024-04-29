using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanketomat.Domain.Repository;

public interface IUnitOfWork : IDisposable
{
    IStudentRepository StudentRepository { get; }
    IProfesorRepository ProfesorRepository { get; }
    IAsistentRepository AsistentRepository { get; }
    IPredmetRepository PredmetRepository { get; }
    IAkreditacijaRepository AkreditacijaRepository { get; }
    ISmerRepository SmerRepository { get; }

    int Save();
}