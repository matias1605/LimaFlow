using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimaFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixCaseSensitivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZonaId",
                table: "vias",
                newName: "zonaid");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "vias",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vias",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "zonaid",
                table: "vias",
                newName: "ZonaId");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "vias",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "vias",
                newName: "Id");
        }
    }
}
