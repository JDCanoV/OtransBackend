using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class nuevoscamposviaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Peso",
                table: "Viaje",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Viaje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Viaje");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Viaje");
        }
    }
}
