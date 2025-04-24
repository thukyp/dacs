using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class PhieuXuat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuXuats",
                columns: table => new
                {
                    MaPhieuXuat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayXuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDoXuat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuXuats", x => x.MaPhieuXuat);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuXuats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhieuXuat = table.Column<int>(type: "int", nullable: false),
                    M_LoaiSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_DonViTinh = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    SoLuong = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuXuats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuats_DonViTinhs_M_DonViTinh",
                        column: x => x.M_DonViTinh,
                        principalTable: "DonViTinhs",
                        principalColumn: "M_DonViTinh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuats_LoaiSanPhams_M_LoaiSP",
                        column: x => x.M_LoaiSP,
                        principalTable: "LoaiSanPhams",
                        principalColumn: "M_LoaiSP",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuats_PhieuXuats_MaPhieuXuat",
                        column: x => x.MaPhieuXuat,
                        principalTable: "PhieuXuats",
                        principalColumn: "MaPhieuXuat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuats_M_DonViTinh",
                table: "ChiTietPhieuXuats",
                column: "M_DonViTinh");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuats_M_LoaiSP",
                table: "ChiTietPhieuXuats",
                column: "M_LoaiSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuats_MaPhieuXuat",
                table: "ChiTietPhieuXuats",
                column: "MaPhieuXuat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietPhieuXuats");

            migrationBuilder.DropTable(
                name: "PhieuXuats");
        }
    }
}
