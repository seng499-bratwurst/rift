using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations.FileDb
{
    /// <inheritdoc />
    public partial class NewFileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceDoc",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceDoc",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Files");
        }
    }
}
