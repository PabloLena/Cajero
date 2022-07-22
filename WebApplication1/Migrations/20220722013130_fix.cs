using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdMovimiento",
                table: "Operaciones");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Operaciones");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Movimientos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMovimiento",
                table: "Operaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Operaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Movimientos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
