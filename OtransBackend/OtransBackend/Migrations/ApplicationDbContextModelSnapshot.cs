﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OtransBackend.Utilities;

#nullable disable

namespace OtransBackend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OtransBackend.Models.Auditoria", b =>
                {
                    b.Property<int>("IdAuditoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAuditoria"));

                    b.Property<string>("Accion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Consulta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("IdAuditoria");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Auditoria", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Calificacion", b =>
                {
                    b.Property<int>("IdCalificacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCalificacion"));

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int>("Puntuaje")
                        .HasColumnType("int");

                    b.HasKey("IdCalificacion");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Calificacion", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Carga", b =>
                {
                    b.Property<int>("IdCarga")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCarga"));

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<byte[]>("Imagen")
                        .HasColumnType("varbinary(max)");

                    b.Property<double>("Peso")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCarga");

                    b.HasIndex("IdEstado");

                    b.ToTable("Carga", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Estado", b =>
                {
                    b.Property<int>("IdEstado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEstado"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEstado");

                    b.ToTable("Estado", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Notificacion", b =>
                {
                    b.Property<int>("IdNotificacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdNotificacion"));

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Mensaje")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdNotificacion");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Notificacion", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Pago", b =>
                {
                    b.Property<int>("IdPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPago"));

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date");

                    b.Property<int?>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdTransportista")
                        .HasColumnType("int");

                    b.Property<string>("MetodoPago")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Propina")
                        .HasColumnType("float");

                    b.Property<double>("Valor")
                        .HasColumnType("float");

                    b.HasKey("IdPago");

                    b.HasIndex("IdEmpresa");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdTransportista");

                    b.ToTable("Pago", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Rol", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdRol");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdEstadoNavigationIdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int");

                    b.Property<byte[]>("Licencia")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Nit")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreEmpresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumCuenta")
                        .HasColumnType("int");

                    b.Property<int>("NumIdentificacion")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonoSos")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUsuario");

                    b.HasIndex("IdEstadoNavigationIdEstado");

                    b.HasIndex("IdRol");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Vehiculo", b =>
                {
                    b.Property<int>("IdVehiculo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdVehiculo"));

                    b.Property<string>("CapacidadCarga")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Carroceria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdEstadoNavigationIdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdTransportista")
                        .HasColumnType("int");

                    b.Property<byte[]>("LicenciaTransito")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("NombreDueño")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumIdentDueño")
                        .HasColumnType("int");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Soat")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Tecnicomecanica")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("TelDueño")
                        .HasColumnType("int");

                    b.HasKey("IdVehiculo");

                    b.HasIndex("IdEstadoNavigationIdEstado");

                    b.HasIndex("IdTransportista");

                    b.ToTable("Vehiculo", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Viaje", b =>
                {
                    b.Property<int>("IdViaje")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdViaje"));

                    b.Property<string>("Destino")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Distancia")
                        .HasColumnType("float");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date");

                    b.Property<int?>("IdCarga")
                        .HasColumnType("int");

                    b.Property<int?>("IdCargaNavigationIdCarga")
                        .HasColumnType("int");

                    b.Property<int?>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int?>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int?>("IdTransportista")
                        .HasColumnType("int");

                    b.Property<string>("Origen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdViaje");

                    b.HasIndex("IdCargaNavigationIdCarga");

                    b.HasIndex("IdEmpresa");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdTransportista");

                    b.ToTable("Viaje", (string)null);
                });

            modelBuilder.Entity("OtransBackend.Models.Auditoria", b =>
                {
                    b.HasOne("OtransBackend.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Auditoria")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Calificacion", b =>
                {
                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Calificacions")
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OtransBackend.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Calificacions")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdEstadoNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Carga", b =>
                {
                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Cargas")
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdEstadoNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Notificacion", b =>
                {
                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Notificacions")
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OtransBackend.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Notificacions")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdEstadoNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Pago", b =>
                {
                    b.HasOne("OtransBackend.Models.Usuario", "IdEmpresaNavigation")
                        .WithMany("PagoIdEmpresaNavigations")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Pagos")
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OtransBackend.Models.Usuario", "IdTransportistaNavigation")
                        .WithMany("PagoIdTransportistaNavigations")
                        .HasForeignKey("IdTransportista")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdEmpresaNavigation");

                    b.Navigation("IdEstadoNavigation");

                    b.Navigation("IdTransportistaNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Usuario", b =>
                {
                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdEstadoNavigationIdEstado");

                    b.HasOne("OtransBackend.Models.Rol", "IdRolNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdEstadoNavigation");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Vehiculo", b =>
                {
                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Vehiculos")
                        .HasForeignKey("IdEstadoNavigationIdEstado");

                    b.HasOne("OtransBackend.Models.Usuario", "IdTransportistaNavigation")
                        .WithMany("Vehiculos")
                        .HasForeignKey("IdTransportista")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdEstadoNavigation");

                    b.Navigation("IdTransportistaNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Viaje", b =>
                {
                    b.HasOne("OtransBackend.Models.Carga", "IdCargaNavigation")
                        .WithMany("Viajes")
                        .HasForeignKey("IdCargaNavigationIdCarga");

                    b.HasOne("OtransBackend.Models.Usuario", "IdEmpresaNavigation")
                        .WithMany("ViajeIdEmpresaNavigations")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OtransBackend.Models.Estado", "IdEstadoNavigation")
                        .WithMany("Viajes")
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("OtransBackend.Models.Usuario", "IdTransportistaNavigation")
                        .WithMany("ViajeIdTransportistaNavigations")
                        .HasForeignKey("IdTransportista")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IdCargaNavigation");

                    b.Navigation("IdEmpresaNavigation");

                    b.Navigation("IdEstadoNavigation");

                    b.Navigation("IdTransportistaNavigation");
                });

            modelBuilder.Entity("OtransBackend.Models.Carga", b =>
                {
                    b.Navigation("Viajes");
                });

            modelBuilder.Entity("OtransBackend.Models.Estado", b =>
                {
                    b.Navigation("Calificacions");

                    b.Navigation("Cargas");

                    b.Navigation("Notificacions");

                    b.Navigation("Pagos");

                    b.Navigation("Usuarios");

                    b.Navigation("Vehiculos");

                    b.Navigation("Viajes");
                });

            modelBuilder.Entity("OtransBackend.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("OtransBackend.Models.Usuario", b =>
                {
                    b.Navigation("Auditoria");

                    b.Navigation("Calificacions");

                    b.Navigation("Notificacions");

                    b.Navigation("PagoIdEmpresaNavigations");

                    b.Navigation("PagoIdTransportistaNavigations");

                    b.Navigation("Vehiculos");

                    b.Navigation("ViajeIdEmpresaNavigations");

                    b.Navigation("ViajeIdTransportistaNavigations");
                });
#pragma warning restore 612, 618
        }
    }
}
