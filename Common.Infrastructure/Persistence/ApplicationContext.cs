using Common.Domain.Entities.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Persistence
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {

        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Genero> Generos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
            } 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.GenId);

                entity.ToTable("Genero");

                entity.Property(e => e.GenId)
                    .HasColumnName("Gen_Id")
                    .HasIdentityOptions(null, null, null, 999999L);

                entity.Property(e => e.GesDescripcion)
                            .HasMaxLength(50)
                            .HasColumnName("Ges_Descripcion");

                entity.Property(e => e.GenActivo)
                            .HasColumnName("Gen_Activo")
                            .HasDefaultValueSql("true");
            });
        }
    }
}
