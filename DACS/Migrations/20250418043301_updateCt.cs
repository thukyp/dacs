using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class updateCt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "M_CTDatHang",
                table: "ChiTietDatHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "ChiTietDatHangs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiDonHang",
                table: "ChiTietDatHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "M_CTDatHang",
                table: "ChiTietDatHangs");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "ChiTietDatHangs");

            migrationBuilder.DropColumn(
                name: "TrangThaiDonHang",
                table: "ChiTietDatHangs");
        }
    }
}
