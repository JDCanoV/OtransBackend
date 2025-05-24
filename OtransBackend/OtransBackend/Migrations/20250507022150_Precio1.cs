using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class Precio1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Precio",
                table: "Viaje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Viaje");
        }
    }
}
