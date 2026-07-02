using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LimaFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaZonas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "vias",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "zonas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zonas", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vias_zonaid",
                table: "vias",
                column: "zonaid");

            migrationBuilder.AddForeignKey(
                name: "FK_vias_zonas_zonaid",
                table: "vias",
                column: "zonaid",
                principalTable: "zonas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vias_zonas_zonaid",
                table: "vias");

            migrationBuilder.DropTable(
                name: "zonas");

            migrationBuilder.DropIndex(
                name: "IX_vias_zonaid",
                table: "vias");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "vias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
