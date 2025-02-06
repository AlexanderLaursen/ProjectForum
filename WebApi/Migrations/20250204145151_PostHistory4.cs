using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PostHistory4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "PostHistories");

            migrationBuilder.DropColumn(
                name: "Edited",
                table: "PostHistories");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "PostHistories");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "PostHistories");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PostHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PostHistories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Edited",
                table: "PostHistories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "PostHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "PostHistories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PostHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
