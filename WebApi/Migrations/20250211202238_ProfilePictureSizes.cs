using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePictureSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                newName: "SmProfilePicture");

            migrationBuilder.AddColumn<string>(
                name: "LgProfilePicture",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MdProfilePicture",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LgProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MdProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SmProfilePicture",
                table: "AspNetUsers",
                newName: "ProfilePictureUrl");
        }
    }
}
