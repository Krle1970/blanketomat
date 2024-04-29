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