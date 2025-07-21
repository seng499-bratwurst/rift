using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations.FileDb
{
    /// <inheritdoc />
    public partial class FileTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Files",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
