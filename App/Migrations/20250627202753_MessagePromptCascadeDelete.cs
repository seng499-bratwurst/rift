using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rift.Migrations
{
    /// <inheritdoc />
    public partial class MessagePromptCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_PromptMessageId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEdges_TargetMessageId",
                table: "MessageEdges",
                column: "TargetMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEdges_Messages_TargetMessageId",
                table: "MessageEdges",
                column: "TargetMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_PromptMessageId",
                table: "Messages",
                column: "PromptMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageEdges_Messages_TargetMessageId",
                table: "MessageEdges");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_PromptMessageId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_MessageEdges_TargetMessageId",
                table: "MessageEdges");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_PromptMessageId",
                table: "Messages",
                column: "PromptMessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
