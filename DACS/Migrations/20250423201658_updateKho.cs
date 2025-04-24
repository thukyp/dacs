using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class updateKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_KhoLuuTrus_M_KhoLuuTru",
                table: "SanPhams");

            migrationBuilder.DropTable(
                name: "KhoLuuTrus");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_M_KhoLuuTru",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "M_KhoLuuTru",
                table: "SanPhams");

            migrationBuilder.CreateTable(
                name: "KhoHangs",
                columns: table => new
                {
                    MaKho = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SucChuaTomTat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoHangs", x => x.MaKho);
                });

            migrationBuilder.CreateTable(
                name: "TonKhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKho = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<long>(type: "bigint", nullable: false),
                    NgayNhapKho = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoLo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    M_DonViTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaDonViTinh = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TonKhos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TonKhos_DonViTinhs_MaDonViTinh",
                        column: x => x.MaDonViTinh,
                        principalTable: "DonViTinhs",
                        principalColumn: "M_DonViTinh");
                    table.ForeignKey(
                        name: "FK_TonKhos_KhoHangs_MaKho",
                        column: x => x.MaKho,
                        principalTable: "KhoHangs",
                        principalColumn: "MaKho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TonKhos_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "M_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TonKhos_MaDonViTinh",
                table: "TonKhos",
                column: "MaDonViTinh");

            migrationBuilder.CreateIndex(
                name: "IX_TonKhos_MaKho",
                table: "TonKhos",
                column: "MaKho");

            migrationBuilder.CreateIndex(
                name: "IX_TonKhos_MaSanPham",
                table: "TonKhos",
                column: "MaSanPham");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TonKhos");

            migrationBuilder.DropTable(
                name: "KhoHangs");

            migrationBuilder.AddColumn<string>(
                name: "M_KhoLuuTru",
                table: "SanPhams",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "KhoLuuTrus",
                columns: table => new
                {
                    M_KhoLuuTru = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongTon = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoLuuTrus", x => x.M_KhoLuuTru);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_M_KhoLuuTru",
                table: "SanPhams",
                column: "M_KhoLuuTru");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_KhoLuuTrus_M_KhoLuuTru",
                table: "SanPhams",
                column: "M_KhoLuuTru",
                principalTable: "KhoLuuTrus",
                principalColumn: "M_KhoLuuTru",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
