using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class updateURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "NongDans",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "NongDans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "NguoiMuas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "NguoiMuas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "NongDans");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "NongDans");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "NguoiMuas");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "NguoiMuas");
        }
    }
}
