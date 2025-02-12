using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DefaultUrlAndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SmProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                defaultValue: "https://projectforum321321321.blob.core.windows.net/profile-pictures/resized/default_50.jpg",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MdProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                defaultValue: "https://projectforum321321321.blob.core.windows.net/profile-pictures/resized/default_100.jpg",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LgProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                defaultValue: "https://projectforum321321321.blob.core.windows.net/profile-pictures/resized/default_300.jpg",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SmProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "https://projectforum321321321.blob.core.windows.net/profile-pictures/resized/default_50.jpg");

            migrationBuilder.AlterColumn<string>(
                name: "MdProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "https://projectforum321321321.blob.core.windows.net/profile-pictures/resized/default_100.jpg");

            migrationBuilder.AlterColumn<string>(
                name: "LgProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "https://projectforum321321321.blob.core.windows.net/profile-pictures/resized/default_300.jpg");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
