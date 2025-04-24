using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class deleteQuanLyInCTTG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThuGoms_QuanLys_M_QuanLy",
                table: "ChiTietThuGoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietThuGoms_M_QuanLy",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "M_QuanLy",
                table: "ChiTietThuGoms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "M_QuanLy",
                table: "ChiTietThuGoms",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_M_QuanLy",
                table: "ChiTietThuGoms",
                column: "M_QuanLy");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThuGoms_QuanLys_M_QuanLy",
                table: "ChiTietThuGoms",
                column: "M_QuanLy",
                principalTable: "QuanLys",
                principalColumn: "M_QuanLy",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
