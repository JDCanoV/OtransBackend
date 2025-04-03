using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtransBackend.Migrations
{
    /// <inheritdoc />
    public partial class final11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Contrasena",
                table: "Usuario",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(47)",
                oldUnicode: false,
                oldMaxLength: 47);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Contrasena",
                table: "Usuario",
                type: "varchar(47)",
                unicode: false,
                maxLength: 47,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);
        }
    }
}
