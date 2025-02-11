using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DBSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "PostHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostHistory_CategoryId",
                table: "PostHistory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostHistory_Categories_CategoryId",
                table: "PostHistory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostHistory_Categories_CategoryId",
                table: "PostHistory");

            migrationBuilder.DropIndex(
                name: "IX_PostHistory_CategoryId",
                table: "PostHistory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PostHistory");
        }
    }
}
