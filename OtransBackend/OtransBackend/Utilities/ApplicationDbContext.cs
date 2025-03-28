using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;
using OtransBackend.Models;

namespace OtransBackend.Utilities
{
    public class ApplicationDbContext : DbContext
    {
        
        
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<Estado> Estado { get; set; }
            public DbSet<Rol> Rol { get; set; }
            public DbSet<Usuario> Usuario { get; set; }
            public DbSet<Vehiculo> Vehiculo { get; set; }
            public DbSet<Carga> Carga { get; set; }
            public DbSet<Viaje> Viaje { get; set; }
            public DbSet<Auditoria> Auditoria { get; set; }
            public DbSet<Notificacion> Notificacion { get; set; }
            public DbSet<Pago> Pago { get; set; }
            public DbSet<Calificacion> Calificacion { get; set; }

            // Configuración de relaciones
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Cambiar la configuración para cada entidad, mapeando las tablas en singular
                modelBuilder.Entity<Auditoria>().ToTable("Auditoria");
                modelBuilder.Entity<Calificacion>().ToTable("Calificacion");
                modelBuilder.Entity<Carga>().ToTable("Carga");
                modelBuilder.Entity<Estado>().ToTable("Estado");
                modelBuilder.Entity<Rol>().ToTable("Rol");
                modelBuilder.Entity<Usuario>().ToTable("Usuario");
                modelBuilder.Entity<Vehiculo>().ToTable("Vehiculo");
                modelBuilder.Entity<Viaje>().ToTable("Viaje");
                modelBuilder.Entity<Notificacion>().ToTable("Notificacion");
                modelBuilder.Entity<Pago>().ToTable("Pago");
            // Configurar las claves primarias para cada entidad
            modelBuilder.Entity<Auditoria>().HasKey(a => a.IdAuditoria);
            modelBuilder.Entity<Calificacion>().HasKey(c => c.IdCalificacion);
            modelBuilder.Entity<Carga>().HasKey(c => c.IdCarga);
            modelBuilder.Entity<Estado>().HasKey(e => e.IdEstado);
            modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Vehiculo>().HasKey(v => v.IdVehiculo);
            modelBuilder.Entity<Viaje>().HasKey(v => v.IdViaje);
            modelBuilder.Entity<Notificacion>().HasKey(n => n.IdNotificacion);
            modelBuilder.Entity<Pago>().HasKey(p => p.IdPago);

            // Relación Usuario → Vehiculo (como transportista)
            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.IdTransportistaNavigation)
                .WithMany(u => u.Vehiculos)
                .HasForeignKey(v => v.IdTransportista)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario → Viaje (como transportista)
            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.IdTransportistaNavigation)
                .WithMany(u => u.ViajeIdTransportistaNavigations)
                .HasForeignKey(v => v.IdTransportista)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario → Viaje (como empresa)
            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.IdEmpresaNavigation)
                .WithMany(u => u.ViajeIdEmpresaNavigations)
                .HasForeignKey(v => v.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Pago → Empresa (Usuario)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.IdEmpresaNavigation)
                .WithMany(u => u.PagoIdEmpresaNavigations)
                .HasForeignKey(p => p.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Pago → Transportista (Usuario)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.IdTransportistaNavigation)
                .WithMany(u => u.PagoIdTransportistaNavigations)
                .HasForeignKey(p => p.IdTransportista)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario → Notificaciones
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.IdUsuarioNavigation)
                .WithMany(u => u.Notificacions)
                .HasForeignKey(n => n.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario → Auditoria
            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.IdUsuarioNavigation)
                .WithMany(u => u.Auditoria)
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario → Calificación
            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.IdUsuarioNavigation)
                .WithMany(u => u.Calificacions)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Carga → Estado
            modelBuilder.Entity<Carga>()
                .HasOne(c => c.IdEstadoNavigation)
                .WithMany(e => e.Cargas)
                .HasForeignKey(c => c.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Viaje → Estado
            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.IdEstadoNavigation)
                .WithMany(e => e.Viajes)
                .HasForeignKey(v => v.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Pago → Estado
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.IdEstadoNavigation)
                .WithMany(e => e.Pagos)
                .HasForeignKey(p => p.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Notificación → Estado
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.IdEstadoNavigation)
                .WithMany(e => e.Notificacions)
                .HasForeignKey(n => n.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Calificación → Estado
            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.IdEstadoNavigation)
                .WithMany(e => e.Calificacions)
                .HasForeignKey(c => c.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario → Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.IdRolNavigation)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
    }
