using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;
using OtransBackend.Models;

namespace OtransBackend.Utilities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Carga> Cargas { get; set; }
        public DbSet<Viaje> Viajes { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

        // Configuración de relaciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurar la clave primaria para Auditoria
            // Configurar la clave primaria para Auditoria
            modelBuilder.Entity<Auditoria>()
                .HasKey(a => a.IdAuditoria);

            // Configurar la clave primaria para Calificacion
            modelBuilder.Entity<Calificacion>()
                .HasKey(c => c.IdCalificacion);

            // Configurar la clave primaria para Carga
            modelBuilder.Entity<Carga>()
                .HasKey(c => c.IdCarga);

            // Configurar la clave primaria para Estado
            modelBuilder.Entity<Estado>()
                .HasKey(e => e.IdEstado);

            // Configurar la clave primaria para Rol
            modelBuilder.Entity<Rol>()
                .HasKey(r => r.IdRol);

            // Configurar la clave primaria para Usuario
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.IdUsuario);

            // Configurar la clave primaria para Vehiculo
            modelBuilder.Entity<Vehiculo>()
                .HasKey(v => v.IdVehiculo);

            // Configurar la clave primaria para Viaje
            modelBuilder.Entity<Viaje>()
                .HasKey(v => v.IdViaje);

            // Configurar la clave primaria para Notificacion
            modelBuilder.Entity<Notificacion>()
                .HasKey(n => n.IdNotificacion);

            // Configurar la clave primaria para Pago
            modelBuilder.Entity<Pago>()
                .HasKey(p => p.IdPago);

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
