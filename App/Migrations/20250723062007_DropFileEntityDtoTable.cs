using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations
{
    /// <inheritdoc />
    public partial class DropFileEntityDtoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                    name: "FileEntityDto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
