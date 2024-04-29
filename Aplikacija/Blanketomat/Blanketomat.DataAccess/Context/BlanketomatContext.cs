using Blanketomat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanketomat.DataAccess.Context;

public class BlanketomatContext : DbContext
{
    public BlanketomatContext(DbContextOptions<BlanketomatContext> options) : base(options)
    {
        
    }

    public DbSet<Student> Studenti { get; set; }
    public DbSet<Predmet> Predmeti { get; set; }
    public DbSet<Profesor> Profesori { get; set; }
    public DbSet<Asistent> Asistenti { get; set; }
    public DbSet<Smer> Smerovi { get; set; }
    public DbSet<Akreditacija> Akreditacije { get; set; }
}