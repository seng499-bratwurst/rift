using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations.FileDb
{
    /// <inheritdoc />
    public partial class AddSourceLinkAndSourceTypeToFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceLink",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceLink",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "Files");
        }
    }
}
