using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations
{
    /// <inheritdoc />
    public partial class MessageEdgeTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageTwoId",
                table: "MessageEdges",
                newName: "TargetMessageId");

            migrationBuilder.RenameColumn(
                name: "MessageOneId",
                table: "MessageEdges",
                newName: "SourceMessageId");

            migrationBuilder.AddColumn<string>(
                name: "SourceHandle",
                table: "MessageEdges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetHandle",
                table: "MessageEdges",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceHandle",
                table: "MessageEdges");

            migrationBuilder.DropColumn(
                name: "TargetHandle",
                table: "MessageEdges");

            migrationBuilder.RenameColumn(
                name: "TargetMessageId",
                table: "MessageEdges",
                newName: "MessageTwoId");

            migrationBuilder.RenameColumn(
                name: "SourceMessageId",
                table: "MessageEdges",
                newName: "MessageOneId");
        }
    }
}
