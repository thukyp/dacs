using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class PhieuXuatAddMaKho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaKho",
                table: "PhieuXuats",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuats_MaKho",
                table: "PhieuXuats",
                column: "MaKho");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuXuats_KhoHangs_MaKho",
                table: "PhieuXuats",
                column: "MaKho",
                principalTable: "KhoHangs",
                principalColumn: "MaKho",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuXuats_KhoHangs_MaKho",
                table: "PhieuXuats");

            migrationBuilder.DropIndex(
                name: "IX_PhieuXuats_MaKho",
                table: "PhieuXuats");

            migrationBuilder.DropColumn(
                name: "MaKho",
                table: "PhieuXuats");
        }
    }
}
