using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_load_playground.Migrations
{
    /// <inheritdoc />
    public partial class Inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transacao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    valor = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    descricao = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transacao", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_transacao_cliente_id",
                table: "transacao",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_transacao_data",
                table: "transacao",
                column: "data");

            migrationBuilder.CreateIndex(
                name: "ix_transacao_data_atualizacao",
                table: "transacao",
                column: "data_atualizacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transacao");
        }
    }
}
