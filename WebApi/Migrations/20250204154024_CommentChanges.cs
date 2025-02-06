using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class CommentChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edited",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostHistory",
                table: "PostHistory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CommentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentHistory_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentHistory_CommentId",
                table: "CommentHistory",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentHistory_UserId",
                table: "CommentHistory",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "CommentHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostHistory",
                table: "PostHistory");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Edited",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Comments");

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
    }
}
