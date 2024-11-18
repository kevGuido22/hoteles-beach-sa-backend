using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelesBeachSABackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Tipo_Cedula = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Nombre_Completo = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Telefono = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
