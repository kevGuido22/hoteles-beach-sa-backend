using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelesBeachSABackend.Migrations
{
    /// <inheritdoc />
    public partial class DetallePagoIdWasAddedInFacturaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "DetallesPagos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre_Completo",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Usuarios",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DetallePagoId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetallePagoId",
                table: "Facturas");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre_Completo",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "DetallesPagos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
