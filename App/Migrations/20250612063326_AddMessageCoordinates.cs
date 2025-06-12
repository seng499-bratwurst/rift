using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "XCoordinate",
                table: "Messages",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "YCoordinate",
                table: "Messages",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XCoordinate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "YCoordinate",
                table: "Messages");
        }
    }
}
