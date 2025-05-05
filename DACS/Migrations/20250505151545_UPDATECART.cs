using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class UPDATECART : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "ChiTietDatHangs");

            migrationBuilder.AlterColumn<string>(
                name: "M_KhachHang",
                table: "DonHangs",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "ChuThich",
                table: "DonHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiShip",
                table: "DonHangs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "TongTien",
                table: "DonHangs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Khoiluong",
                table: "ChiTietDatHangs",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChuThich",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "DiaChiShip",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "Khoiluong",
                table: "ChiTietDatHangs");

            migrationBuilder.AlterColumn<string>(
                name: "M_KhachHang",
                table: "DonHangs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "ChiTietDatHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
