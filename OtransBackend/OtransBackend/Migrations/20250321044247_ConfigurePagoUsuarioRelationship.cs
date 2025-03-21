using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurePagoUsuarioRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.IdEstado);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Cargas",
                columns: table => new
                {
                    IdCarga = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Peso = table.Column<double>(type: "float", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargas", x => x.IdCarga);
                    table.ForeignKey(
                        name: "FK_Cargas_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumIdentificacion = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<int>(type: "int", nullable: true),
                    TelefonoSos = table.Column<int>(type: "int", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumCuenta = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Licencia = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Nit = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IdRol = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdEstadoNavigationIdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Estados_IdEstadoNavigationIdEstado",
                        column: x => x.IdEstadoNavigationIdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Consulta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.IdAuditoria);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    IdCalificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Puntuaje = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.IdCalificacion);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Propina = table.Column<double>(type: "float", nullable: true),
                    IdTransportista = table.Column<int>(type: "int", nullable: true),
                    IdEmpresa = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_IdTransportista",
                        column: x => x.IdTransportista,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapacidadCarga = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soat = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Tecnicomecanica = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LicenciaTransito = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NombreDueño = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumIdentDueño = table.Column<int>(type: "int", nullable: true),
                    TelDueño = table.Column<int>(type: "int", nullable: true),
                    Carroceria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdTransportista = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdEstadoNavigationIdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Estados_IdEstadoNavigationIdEstado",
                        column: x => x.IdEstadoNavigationIdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK_Vehiculos_Usuarios_IdTransportista",
                        column: x => x.IdTransportista,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Viajes",
                columns: table => new
                {
                    IdViaje = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distancia = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdCarga = table.Column<int>(type: "int", nullable: true),
                    IdTransportista = table.Column<int>(type: "int", nullable: true),
                    IdEmpresa = table.Column<int>(type: "int", nullable: true),
                    IdCargaNavigationIdCarga = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viajes", x => x.IdViaje);
                    table.ForeignKey(
                        name: "FK_Viajes_Cargas_IdCargaNavigationIdCarga",
                        column: x => x.IdCargaNavigationIdCarga,
                        principalTable: "Cargas",
                        principalColumn: "IdCarga");
                    table.ForeignKey(
                        name: "FK_Viajes_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Viajes_Usuarios_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Viajes_Usuarios_IdTransportista",
                        column: x => x.IdTransportista,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_IdUsuario",
                table: "Auditorias",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_IdEstado",
                table: "Calificaciones",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_IdUsuario",
                table: "Calificaciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cargas_IdEstado",
                table: "Cargas",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_IdEstado",
                table: "Notificaciones",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_IdUsuario",
                table: "Notificaciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdEmpresa",
                table: "Pagos",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdEstado",
                table: "Pagos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdTransportista",
                table: "Pagos",
                column: "IdTransportista");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdEstadoNavigationIdEstado",
                table: "Usuarios",
                column: "IdEstadoNavigationIdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_IdEstadoNavigationIdEstado",
                table: "Vehiculos",
                column: "IdEstadoNavigationIdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_IdTransportista",
                table: "Vehiculos",
                column: "IdTransportista");

            migrationBuilder.CreateIndex(
                name: "IX_Viajes_IdCargaNavigationIdCarga",
                table: "Viajes",
                column: "IdCargaNavigationIdCarga");

            migrationBuilder.CreateIndex(
                name: "IX_Viajes_IdEmpresa",
                table: "Viajes",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Viajes_IdEstado",
                table: "Viajes",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Viajes_IdTransportista",
                table: "Viajes",
                column: "IdTransportista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Viajes");

            migrationBuilder.DropTable(
                name: "Cargas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
