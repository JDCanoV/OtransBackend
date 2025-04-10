using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class ADDcedula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ArchiDocu",
                table: "Usuario",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchiDocu",
                table: "Usuario");
        }
    }
}
