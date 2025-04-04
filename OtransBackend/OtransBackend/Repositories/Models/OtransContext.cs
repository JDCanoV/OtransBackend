using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OtransBackend.Repositories.Models;

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
        => optionsBuilder.UseSqlServer("Server=OBLAANC;Database=Otrans;User Id=User_Otrans;Password=User_Otrans;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA0F93BAB2A");

            entity.Property(e => e.IdAuditoria).ValueGeneratedNever();
            entity.Property(e => e.Accion)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Consulta)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
            

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Auditoria__IdUsu__3A81B327");
        });

        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("PK__Califica__40E4A7516CF403F6");

            entity.ToTable("Calificacion");

            entity.Property(e => e.IdCalificacion).ValueGeneratedNever();
            entity.Property(e => e.Comentario)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario");
            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Calificac__IdEst__45F365D3");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Calificac__IdUsu__46E78A0C");
        });

        modelBuilder.Entity<Carga>(entity =>
        {
            entity.HasKey(e => e.IdCarga).HasName("PK__Carga__6C9856177E87A551");

            entity.ToTable("Carga");

            entity.Property(e => e.IdCarga).ValueGeneratedNever();
            entity.Property(e => e.Tipo)
                .HasMaxLength(47)
                .IsUnicode(false);

            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");
            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Cargas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Carga__IdEstado__31EC6D26");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__FBB0EDC17C858234");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(47)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__Notifica__F6CA0A85B4D735BE");

            entity.ToTable("Notificacion");

            entity.Property(e => e.IdNotificacion).ValueGeneratedNever();
            entity.Property(e => e.Mensaje)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");
            entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Notificac__IdEst__3E52440B");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Notificac__IdUsu__3D5E1FD2");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__FC851A3AAF9094F3");

            entity.ToTable("Pago");

            entity.Property(e => e.IdPago).ValueGeneratedNever();
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(47)
                .IsUnicode(false);

            entity.Property(e => e.IdEmpresa).HasColumnName("IdEmpresa");
            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");
            entity.Property(e => e.IdTransportista).HasColumnName("IdTransportista");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.PagoIdEmpresaNavigations)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Pago__IdEmpresa__4222D4EF");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Pago__IdEstado__4316F928");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.PagoIdTransportistaNavigations)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__Pago__IdTranspor__412EB0B6");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C97631D7D");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(47)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF970F68F829");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A191D2A9EE5").IsUnique();

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();
            entity.Property(e => e.Apellido)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nit).HasColumnName("NIT");
            entity.Property(e => e.Nombre)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoSos)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");
            entity.Property(e => e.IdRol).HasColumnName("IdRol");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Usuario__IdEstad__2A4B4B5E");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__IdRol__29572725");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__7086121535148C26");

            entity.ToTable("Vehiculo");

            entity.HasIndex(e => e.Placa, "UQ__Vehiculo__8310F99DE7F01474").IsUnique();

            entity.Property(e => e.IdVehiculo).ValueGeneratedNever();
            entity.Property(e => e.CapacidadCarga)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Carroceria)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.NombreDueño)
                .HasMaxLength(47)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Soat).HasColumnName("SOAT");

            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");
            entity.Property(e => e.IdTransportista).HasColumnName("IdTransportista");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Vehiculo__IdEsta__2F10007B");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__Vehiculo__IdTran__2E1BDC42");
        });

        modelBuilder.Entity<Viaje>(entity =>
        {
            entity.HasKey(e => e.IdViaje).HasName("PK__Viaje__580AB6B9ED732581");

            entity.ToTable("Viaje");

            entity.Property(e => e.IdViaje).ValueGeneratedNever();
            entity.Property(e => e.Destino)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Origen)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.IdCarga).HasColumnName("IdCarga");
            entity.Property(e => e.IdEmpresa).HasColumnName("IdEmpresa");

            entity.Property(e => e.IdEstado).HasColumnName("IdEstado");
            entity.Property(e => e.IdTransportista).HasColumnName("IdTransportista");


            entity.HasOne(d => d.IdCargaNavigation).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.IdCarga)
                .HasConstraintName("FK__Viaje__IdCarga__35BCFE0A");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ViajeIdEmpresaNavigations)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Viaje__IdEmpresa__37A5467C");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Viajes)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Viaje__IdEstado__34C8D9D1");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.ViajeIdTransportistaNavigations)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__Viaje__IdTranspo__36B12243");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
