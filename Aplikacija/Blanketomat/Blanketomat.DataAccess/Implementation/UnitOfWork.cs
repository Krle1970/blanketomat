using Blanketomat.DataAccess.Context;
using Blanketomat.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanketomat.DataAccess.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlanketomatContext _context;

    public UnitOfWork(BlanketomatContext context)
    {
        _context = context;
        StudentRepository = new StudentRepository(_context);
        ProfesorRepository = new ProfesorRepository(_context);
        AsistentRepository = new AsistentRepository(_context);
        PredmetRepository = new PredmetRepository(_context);
        AkreditacijaRepository = new AkreditacijaRepository(_context);
        SmerRepository = new SmerRepository(_context);
    }

    public IStudentRepository StudentRepository { get; private set; }

    public IProfesorRepository ProfesorRepository { get; private set; }

    public IAsistentRepository AsistentRepository { get; private set; }

    public IPredmetRepository PredmetRepository { get; private set; }

    public IAkreditacijaRepository AkreditacijaRepository { get; private set; }

    public ISmerRepository SmerRepository { get; private set; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
}