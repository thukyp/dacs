using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMSPInCTTG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThuGoms_SanPhams_M_SanPham",
                table: "ChiTietThuGoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietThuGoms_M_SanPham",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "M_SanPham",
                table: "ChiTietThuGoms");

            migrationBuilder.AddColumn<string>(
                name: "SanPhamM_SanPham",
                table: "ChiTietThuGoms",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_SanPhamM_SanPham",
                table: "ChiTietThuGoms",
                column: "SanPhamM_SanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThuGoms_SanPhams_SanPhamM_SanPham",
                table: "ChiTietThuGoms",
                column: "SanPhamM_SanPham",
                principalTable: "SanPhams",
                principalColumn: "M_SanPham");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThuGoms_SanPhams_SanPhamM_SanPham",
                table: "ChiTietThuGoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietThuGoms_SanPhamM_SanPham",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "SanPhamM_SanPham",
                table: "ChiTietThuGoms");

            migrationBuilder.AddColumn<string>(
                name: "M_SanPham",
                table: "ChiTietThuGoms",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_M_SanPham",
                table: "ChiTietThuGoms",
                column: "M_SanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThuGoms_SanPhams_M_SanPham",
                table: "ChiTietThuGoms",
                column: "M_SanPham",
                principalTable: "SanPhams",
                principalColumn: "M_SanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
