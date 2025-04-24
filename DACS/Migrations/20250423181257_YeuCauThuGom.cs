using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class YeuCauThuGom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "M_QuanLy",
                table: "YeuCauThuGoms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Thoi_Gian_HT",
                table: "YeuCauThuGoms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauThuGoms_M_QuanLy",
                table: "YeuCauThuGoms",
                column: "M_QuanLy");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauThuGoms_AspNetUsers_M_QuanLy",
                table: "YeuCauThuGoms",
                column: "M_QuanLy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauThuGoms_AspNetUsers_M_QuanLy",
                table: "YeuCauThuGoms");

            migrationBuilder.DropIndex(
                name: "IX_YeuCauThuGoms_M_QuanLy",
                table: "YeuCauThuGoms");

            migrationBuilder.DropColumn(
                name: "M_QuanLy",
                table: "YeuCauThuGoms");

            migrationBuilder.DropColumn(
                name: "Thoi_Gian_HT",
                table: "YeuCauThuGoms");
        }
    }
}
