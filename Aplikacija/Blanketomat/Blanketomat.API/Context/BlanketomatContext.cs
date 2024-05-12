using Blanketomat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blanketomat.API.Context;

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
    public DbSet<Administrator> Administratori { get; set; }
    public DbSet<Blanket> Blanketi { get; set; }
    public DbSet<IspitniRok> IspitniRokovi { get; set; }
    public DbSet<Oblast> Oblasti { get; set; }
    public DbSet<Pitanje> Pitanja { get; set; }
    public DbSet<Podoblast> Podoblasti { get; set; }
    public DbSet<PonavljanjeIspitnogRoka> PonavljanjaIspitnihRokova { get; set; }
    public DbSet<Zadatak> Zadaci { get; set; }
    public DbSet<Komentar> Komentari { get; set; }
    public DbSet<Slika> Slike { get; set; }
}