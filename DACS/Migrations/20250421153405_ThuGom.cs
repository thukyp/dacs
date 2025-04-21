using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class ThuGom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThuGoms_KhachHangs_M_KhachHang",
                table: "ChiTietThuGoms");

            migrationBuilder.AddColumn<bool>(
                name: "DacTinh_AmUot",
                table: "ChiTietThuGoms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DacTinh_CongKenh",
                table: "ChiTietThuGoms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DacTinh_DaXuLy",
                table: "ChiTietThuGoms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DacTinh_DoAmCao",
                table: "ChiTietThuGoms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DacTinh_Kho",
                table: "ChiTietThuGoms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DacTinh_TapChat",
                table: "ChiTietThuGoms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DanhSachHinhAnh",
                table: "ChiTietThuGoms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThuGoms_KhachHangs_M_KhachHang",
                table: "ChiTietThuGoms",
                column: "M_KhachHang",
                principalTable: "KhachHangs",
                principalColumn: "M_KhachHang",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThuGoms_KhachHangs_M_KhachHang",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DacTinh_AmUot",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DacTinh_CongKenh",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DacTinh_DaXuLy",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DacTinh_DoAmCao",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DacTinh_Kho",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DacTinh_TapChat",
                table: "ChiTietThuGoms");

            migrationBuilder.DropColumn(
                name: "DanhSachHinhAnh",
                table: "ChiTietThuGoms");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThuGoms_KhachHangs_M_KhachHang",
                table: "ChiTietThuGoms",
                column: "M_KhachHang",
                principalTable: "KhachHangs",
                principalColumn: "M_KhachHang",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
