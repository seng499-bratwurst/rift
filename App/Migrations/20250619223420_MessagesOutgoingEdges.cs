using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations
{
    /// <inheritdoc />
    public partial class MessagesOutgoingEdges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MessageEdges_SourceMessageId",
                table: "MessageEdges",
                column: "SourceMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEdges_Messages_SourceMessageId",
                table: "MessageEdges",
                column: "SourceMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageEdges_Messages_SourceMessageId",
                table: "MessageEdges");

            migrationBuilder.DropIndex(
                name: "IX_MessageEdges_SourceMessageId",
                table: "MessageEdges");
        }
    }
}
