using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_ZExtensionsPlayground.Migrations
{
    public partial class Inicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Documento = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cliente");
        }
    }
}
