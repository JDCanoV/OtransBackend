using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;

namespace OtransBackend.Models;

public partial class OtransContext : DbContext
{
    public OtransContext()
    {
    }

    public OtransContext(DbContextOptions<OtransContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditoria> Auditoria { get; set; }

    public virtual DbSet<Calificacion> Calificacions { get; set; }

    public virtual DbSet<Carga> Cargas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<Viaje> Viajes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BJ4E2PT;Database=Otrans;User Id=User_Otrans;Password=User_Otrans;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__E9F1DAD414B2375C");

            entity.Property(e => e.IdAuditoria)
                .ValueGeneratedNever()
                .HasColumnName("Id_Auditoria");
            entity.Property(e => e.Accion)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Consulta)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Auditoria__Id_Us__3A81B327");
        });

        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("PK__Califica__6F6E6A405EEE93A3");

            entity.ToTable("Calificacion");

            entity.Property(e => e.IdCalificacion)
                .ValueGeneratedNever()
                .HasColumnName("Id_Calificacion");
            entity.Property(e => e.Comentario)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Calificac__Id_Es__45F365D3");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Calificac__Id_Us__46E78A0C");
        });

        modelBuilder.Entity<Carga>(entity =>
        {
            entity.HasKey(e => e.IdCarga).HasName("PK__Carga__4054BDA3B57DB563");

            entity.ToTable("Carga");

            entity.Property(e => e.IdCarga)
                .ValueGeneratedNever()
                .HasColumnName("Id_Carga");
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.Tipo)
                .HasMaxLength(47)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Cargas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Carga__Id_Estado__31EC6D26");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__AB2EB6F818B371C9");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado)
                .ValueGeneratedNever()
                .HasColumnName("Id_Estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(47)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__Notifica__33C2FF16FEF82758");

            entity.ToTable("Notificacion");

            entity.Property(e => e.IdNotificacion)
                .ValueGeneratedNever()
                .HasColumnName("Id_Notificacion");
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(47)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Notificac__Id_Es__3E52440B");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Notificac__Id_Us__3D5E1FD2");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__3E79AD9A66BA2E28");

            entity.ToTable("Pago");

            entity.Property(e => e.IdPago)
                .ValueGeneratedNever()
                .HasColumnName("Id_Pago");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdTransportista).HasColumnName("Id_Transportista");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(47)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.PagoIdEmpresaNavigations)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Pago__Id_Empresa__4222D4EF");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Pago__Id_Estado__4316F928");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.PagoIdTransportistaNavigations)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__Pago__Id_Transpo__412EB0B6");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__55932E8651FD5CBC");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol)
                .ValueGeneratedNever()
                .HasColumnName("Id_Rol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(47)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__63C76BE29933B4F3");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A196101FBB9").IsUnique();

            entity.Property(e => e.IdUsuario)
                .ValueGeneratedNever()
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Nit).HasColumnName("NIT");
            entity.Property(e => e.Nombre)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoSos).HasColumnName("Telefono_SOS");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Usuario__Id_Esta__2A4B4B5E");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__Id_Rol__29572725");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__46DBF4B41EC1FAD8");

            entity.ToTable("Vehiculo");

            entity.HasIndex(e => e.Placa, "UQ__Vehiculo__8310F99DB4A98B1B").IsUnique();

            entity.Property(e => e.IdVehiculo)
                .ValueGeneratedNever()
                .HasColumnName("Id_Vehiculo");
            entity.Property(e => e.CapacidadCarga)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Carroceria)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdTransportista).HasColumnName("Id_Transportista");
            entity.Property(e => e.NombreDueño)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Soat).HasColumnName("SOAT");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Vehiculo__Id_Est__2F10007B");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__Vehiculo__Id_Tra__2E1BDC42");
        });

        modelBuilder.Entity<Viaje>(entity =>
        {
            entity.HasKey(e => e.IdViaje).HasName("PK__Viaje__9BC209F7FFB45073");

            entity.ToTable("Viaje");

            entity.Property(e => e.IdViaje)
                .ValueGeneratedNever()
                .HasColumnName("Id_Viaje");
            entity.Property(e => e.Destino)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdCarga).HasColumnName("Id_Carga");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");
            entity.Property(e => e.IdTransportista).HasColumnName("Id_Transportista");
            entity.Property(e => e.Origen)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCargaNavigation).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.IdCarga)
                .HasConstraintName("FK__Viaje__Id_Carga__35BCFE0A");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ViajeIdEmpresaNavigations)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Viaje__Id_Empres__37A5467C");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Viaje__Id_Estado__34C8D9D1");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.ViajeIdTransportistaNavigations)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__Viaje__Id_Transp__36B12243");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
