using BootcampCLT.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Infraestructure.Context
{
    public class PostegresDbContext : DbContext
    {
        public PostegresDbContext(DbContextOptions<PostegresDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos => Set<Producto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>(b =>
            {
                b.ToTable("productos");
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.Codigo).HasColumnName("codigo").HasMaxLength(50).IsRequired();
                b.Property(p => p.Nombre).HasColumnName("nombre").HasMaxLength(200).IsRequired();
                b.Property(p => p.Descripcion).HasColumnName("descripcion").HasMaxLength(500);
                b.Property(p => p.Precio).HasColumnName("precio").HasColumnType("numeric(18,2)");
                b.Property(p => p.Activo).HasColumnName("activo").IsRequired();
                b.Property(p => p.CategoriaId).HasColumnName("categoria_id").IsRequired();
                b.Property(p => p.FechaCreacion).HasColumnName("fecha_creacion").IsRequired();
                b.Property(p => p.FechaActualizacion).HasColumnName("fecha_actualizacion");
                b.Property(p => p.CantidadStock).HasColumnName("cantidad_stock").HasDefaultValue(0);
            });
        }
    }
}
