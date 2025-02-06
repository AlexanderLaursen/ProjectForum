using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PostHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostHistory_AspNetUsers_UserId",
                table: "PostHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostHistory_Categories_CategoryId",
                table: "PostHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PostHistory_Posts_PostId",
                table: "PostHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostHistory",
                table: "PostHistory");

            migrationBuilder.RenameTable(
                name: "PostHistory",
                newName: "PostHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PostHistory_UserId",
                table: "PostHistories",
                newName: "IX_PostHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostHistory_PostId",
                table: "PostHistories",
                newName: "IX_PostHistories_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostHistory_CategoryId",
                table: "PostHistories",
                newName: "IX_PostHistories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostHistories",
                table: "PostHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistories_AspNetUsers_UserId",
                table: "PostHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistories_Categories_CategoryId",
                table: "PostHistories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistories_Posts_PostId",
                table: "PostHistories",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostHistories_AspNetUsers_UserId",
                table: "PostHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostHistories_Categories_CategoryId",
                table: "PostHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostHistories_Posts_PostId",
                table: "PostHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostHistories",
                table: "PostHistories");

            migrationBuilder.RenameTable(
                name: "PostHistories",
                newName: "PostHistory");

            migrationBuilder.RenameIndex(
                name: "IX_PostHistories_UserId",
                table: "PostHistory",
                newName: "IX_PostHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostHistories_PostId",
                table: "PostHistory",
                newName: "IX_PostHistory_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostHistories_CategoryId",
                table: "PostHistory",
                newName: "IX_PostHistory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostHistory",
                table: "PostHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistory_AspNetUsers_UserId",
                table: "PostHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistory_Categories_CategoryId",
                table: "PostHistory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistory_Posts_PostId",
                table: "PostHistory",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
