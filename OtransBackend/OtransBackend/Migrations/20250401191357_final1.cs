using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class final1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado__FBB0EDC17C858234", x => x.IdEstado);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rol__2A49584C97631D7D", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Carga",
                columns: table => new
                {
                    IdCarga = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Peso = table.Column<double>(type: "float", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Carga__6C9856177E87A551", x => x.IdCarga);
                    table.ForeignKey(
                        name: "FK__Carga__IdEstado__31EC6D26",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    NumIdentificacion = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Apellido = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    TelefonoSos = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Correo = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Contrasena = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    NombreEmpresa = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: true),
                    NumCuenta = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Licencia = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NIT = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IdRol = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__5B65BF970F68F829", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK__Usuario__IdEstad__2A4B4B5E",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Usuario__IdRol__29572725",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Consulta = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Accion = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Auditori__7FD13FA0F93BAB2A", x => x.IdAuditoria);
                    table.ForeignKey(
                        name: "FK__Auditoria__IdUsu__3A81B327",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Calificacion",
                columns: table => new
                {
                    IdCalificacion = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Puntuaje = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Califica__40E4A7516CF403F6", x => x.IdCalificacion);
                    table.ForeignKey(
                        name: "FK__Calificac__IdEst__45F365D3",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Calificac__IdUsu__46E78A0C",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Mensaje = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__F6CA0A85B4D735BE", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK__Notificac__IdEst__3E52440B",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Notificac__IdUsu__3D5E1FD2",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    MetodoPago = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Propina = table.Column<double>(type: "float", nullable: true),
                    IdTransportista = table.Column<int>(type: "int", nullable: true),
                    IdEmpresa = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pago__FC851A3AAF9094F3", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK__Pago__IdEmpresa__4222D4EF",
                        column: x => x.IdEmpresa,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                    table.ForeignKey(
                        name: "FK__Pago__IdEstado__4316F928",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Pago__IdTranspor__412EB0B6",
                        column: x => x.IdTransportista,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CapacidadCarga = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    SOAT = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Tecnicomecanica = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LicenciaTransito = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NombreDueño = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    NumIdentDueño = table.Column<int>(type: "int", nullable: true),
                    TelDueño = table.Column<int>(type: "int", nullable: true),
                    Carroceria = table.Column<string>(type: "varchar(47)", unicode: false, maxLength: 47, nullable: false),
                    IdTransportista = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vehiculo__7086121535148C26", x => x.IdVehiculo);
                    table.ForeignKey(
                        name: "FK__Vehiculo__IdEsta__2F10007B",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Vehiculo__IdTran__2E1BDC42",
                        column: x => x.IdTransportista,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Viaje",
                columns: table => new
                {
                    IdViaje = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Destino = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Origen = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Distancia = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    IdCarga = table.Column<int>(type: "int", nullable: true),
                    IdTransportista = table.Column<int>(type: "int", nullable: true),
                    IdEmpresa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Viaje__580AB6B9ED732581", x => x.IdViaje);
                    table.ForeignKey(
                        name: "FK__Viaje__IdCarga__35BCFE0A",
                        column: x => x.IdCarga,
                        principalTable: "Carga",
                        principalColumn: "IdCarga");
                    table.ForeignKey(
                        name: "FK__Viaje__IdEmpresa__37A5467C",
                        column: x => x.IdEmpresa,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                    table.ForeignKey(
                        name: "FK__Viaje__IdEstado__34C8D9D1",
                        column: x => x.IdEstado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Viaje__IdTranspo__36B12243",
                        column: x => x.IdTransportista,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_IdUsuario",
                table: "Auditoria",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_IdEstado",
                table: "Calificacion",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_IdUsuario",
                table: "Calificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Carga_IdEstado",
                table: "Carga",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdEstado",
                table: "Notificacion",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdUsuario",
                table: "Notificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_IdEmpresa",
                table: "Pago",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_IdEstado",
                table: "Pago",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_IdTransportista",
                table: "Pago",
                column: "IdTransportista");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdEstado",
                table: "Usuario",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdRol",
                table: "Usuario",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuario__60695A191D2A9EE5",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_IdEstado",
                table: "Vehiculo",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculo_IdTransportista",
                table: "Vehiculo",
                column: "IdTransportista");

            migrationBuilder.CreateIndex(
                name: "UQ__Vehiculo__8310F99DE7F01474",
                table: "Vehiculo",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Viaje_IdCarga",
                table: "Viaje",
                column: "IdCarga");

            migrationBuilder.CreateIndex(
                name: "IX_Viaje_IdEmpresa",
                table: "Viaje",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Viaje_IdEstado",
                table: "Viaje",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Viaje_IdTransportista",
                table: "Viaje",
                column: "IdTransportista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Calificacion");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "Vehiculo");

            migrationBuilder.DropTable(
                name: "Viaje");

            migrationBuilder.DropTable(
                name: "Carga");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
