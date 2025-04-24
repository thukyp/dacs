using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTonKhoFKToLoaiSanPham : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TonKhos_SanPhams_MaSanPham",
                table: "TonKhos");

            migrationBuilder.RenameColumn(
                name: "MaSanPham",
                table: "TonKhos",
                newName: "M_LoaiSP");

            migrationBuilder.RenameIndex(
                name: "IX_TonKhos_MaSanPham",
                table: "TonKhos",
                newName: "IX_TonKhos_M_LoaiSP");

            migrationBuilder.AddForeignKey(
                name: "FK_TonKhos_LoaiSanPhams_M_LoaiSP",
                table: "TonKhos",
                column: "M_LoaiSP",
                principalTable: "LoaiSanPhams",
                principalColumn: "M_LoaiSP",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TonKhos_LoaiSanPhams_M_LoaiSP",
                table: "TonKhos");

            migrationBuilder.RenameColumn(
                name: "M_LoaiSP",
                table: "TonKhos",
                newName: "MaSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_TonKhos_M_LoaiSP",
                table: "TonKhos",
                newName: "IX_TonKhos_MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_TonKhos_SanPhams_MaSanPham",
                table: "TonKhos",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "M_SanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
