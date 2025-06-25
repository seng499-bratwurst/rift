using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations
{
    /// <inheritdoc />
    public partial class addedCompantAPIToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspCompanyAPITokens",
                columns: table => new
                {
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    ONCApiToken = table.Column<string>(type: "text", nullable: true),
                    Usage = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspCompanyAPITokens", x => x.CompanyName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspCompanyAPITokens");
        }
    }
}
