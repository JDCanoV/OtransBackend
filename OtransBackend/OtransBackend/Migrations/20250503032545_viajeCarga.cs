using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class viajeCarga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Carga");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Viaje",
                newName: "TipoCarroceria");

            migrationBuilder.RenameColumn(
                name: "Imagen",
                table: "Carga",
                newName: "Imagen9");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Viaje",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Viaje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TamañoVeh",
                table: "Viaje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoCarga",
                table: "Viaje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen1",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen10",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen2",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen3",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen4",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen5",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen6",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen7",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen8",
                table: "Carga",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Viaje");

            migrationBuilder.DropColumn(
                name: "TamañoVeh",
                table: "Viaje");

            migrationBuilder.DropColumn(
                name: "TipoCarga",
                table: "Viaje");

            migrationBuilder.DropColumn(
                name: "Imagen1",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen10",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen2",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen3",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen4",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen5",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen6",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen7",
                table: "Carga");

            migrationBuilder.DropColumn(
                name: "Imagen8",
                table: "Carga");

            migrationBuilder.RenameColumn(
                name: "TipoCarroceria",
                table: "Viaje",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "Imagen9",
                table: "Carga",
                newName: "Imagen");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Fecha",
                table: "Viaje",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<double>(
                name: "Peso",
                table: "Carga",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Carga",
                type: "varchar(47)",
                unicode: false,
                maxLength: 47,
                nullable: false,
                defaultValue: "");
        }
    }
}
