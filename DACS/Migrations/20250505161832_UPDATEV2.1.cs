using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class UPDATEV21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatHangs_KhachHangs_M_KhachHang",
                table: "ChiTietDatHangs");

            migrationBuilder.DropColumn(
                name: "ChuThich",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "DiaChiShip",
                table: "DonHangs");

            migrationBuilder.RenameColumn(
                name: "TongTien",
                table: "DonHangs",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DonHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "DonHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "M_KhachHang",
                table: "ChiTietDatHangs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "ChiTietDatHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ChiTietDatHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatHangs_KhachHangs_M_KhachHang",
                table: "ChiTietDatHangs",
                column: "M_KhachHang",
                principalTable: "KhachHangs",
                principalColumn: "M_KhachHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatHangs_KhachHangs_M_KhachHang",
                table: "ChiTietDatHangs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ChiTietDatHangs");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ChiTietDatHangs");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "DonHangs",
                newName: "TongTien");

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

            migrationBuilder.AlterColumn<string>(
                name: "M_KhachHang",
                table: "ChiTietDatHangs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatHangs_KhachHangs_M_KhachHang",
                table: "ChiTietDatHangs",
                column: "M_KhachHang",
                principalTable: "KhachHangs",
                principalColumn: "M_KhachHang",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
