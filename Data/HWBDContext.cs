using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data
{
    public class HWBDContext : DbContext
    {
        public HWBDContext(DbContextOptions<HWBDContext> options) : base(options)
        {
        }

        public DbSet<Contacto> Contactos => Set<Contacto>();
        public DbSet<Persona> Personas => Set<Persona>();
        public DbSet<Pasaporte> Pasaportes => Set<Pasaporte>();
        public DbSet<Pais> Paises => Set<Pais>();
        public DbSet<Materia> Materias => Set<Materia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Persona>()
                .HasOne(p => p.Pasaporte)
                .WithOne(p => p.Persona)
                .HasForeignKey<Pasaporte>(p => p.personaId);

            modelBuilder.Entity<Pais>()
                .ToTable("Pais");

            modelBuilder.Entity<Pais>()
                .HasMany(p => p.Personas)
                .WithOne(p => p.Pais)
                .HasForeignKey(p => p.PaisId);

            modelBuilder.Entity<Persona>()
                .HasMany(p => p.Materias)
                .WithMany(m => m.Personas)
                .UsingEntity<Dictionary<string, object>>(
                    "Inscripcion",
                    derecha => derecha
                        .HasOne<Materia>()
                        .WithMany()
                        .HasForeignKey("MateriaId"),
                    izquierda => izquierda
                        .HasOne<Persona>()
                        .WithMany()
                        .HasForeignKey("PersonaId"),
                    inscripcion =>
                    {
                        inscripcion.HasKey("PersonaId", "MateriaId");
                        inscripcion.ToTable("Inscripcion");
                    });
        }
    }
}
