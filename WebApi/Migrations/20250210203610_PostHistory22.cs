using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PostHistory22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostHistory_Categories_CategoryId",
                table: "PostHistory");

            migrationBuilder.DropTable(
                name: "CommentHistoryDto");

            migrationBuilder.DropIndex(
                name: "IX_PostHistory_CategoryId",
                table: "PostHistory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PostHistory");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PostHistory",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentHistory");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PostHistory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "PostHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommentHistoryDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommentId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentHistoryDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentHistoryDto_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentHistoryDto_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostHistory_CategoryId",
                table: "PostHistory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentHistoryDto_CommentId",
                table: "CommentHistoryDto",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentHistoryDto_UserId",
                table: "CommentHistoryDto",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistory_Categories_CategoryId",
                table: "PostHistory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
