using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class intaili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonViTinhs",
                columns: table => new
                {
                    M_DonViTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoaiTinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonViTinhs", x => x.M_DonViTinh);
                });

            migrationBuilder.CreateTable(
                name: "GiamGias",
                columns: table => new
                {
                    M_GiamGia = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenGiamGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Tien = table.Column<long>(type: "bigint", nullable: false),
                    LoaiGiamGia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GiaTriDonHangToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiamGias", x => x.M_GiamGia);
                });

            migrationBuilder.CreateTable(
                name: "HoanTras",
                columns: table => new
                {
                    M_HoanTra = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayYeuCau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoanTras", x => x.M_HoanTra);
                });

            migrationBuilder.CreateTable(
                name: "KhoLuuTrus",
                columns: table => new
                {
                    M_KhoLuuTru = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuongTon = table.Column<long>(type: "bigint", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoLuuTrus", x => x.M_KhoLuuTru);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPhams",
                columns: table => new
                {
                    M_LoaiSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPhams", x => x.M_LoaiSP);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToans",
                columns: table => new
                {
                    M_PhuongThuc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenPhuongThuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToans", x => x.M_PhuongThuc);
                });

            migrationBuilder.CreateTable(
                name: "QuanLys",
                columns: table => new
                {
                    M_QuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    XacNhan = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLys", x => x.M_QuanLy);
                });

            migrationBuilder.CreateTable(
                name: "VanChuyens",
                columns: table => new
                {
                    M_VanDon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DonViVanChuyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanChuyens", x => x.M_VanDon);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiMuas",
                columns: table => new
                {
                    M_NguoiMua = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Ten_NguoiMua = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email_NguoiMua = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT_NguoiMua = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChi_NguoiMua = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiMuas", x => x.M_NguoiMua);
                    table.ForeignKey(
                        name: "FK_NguoiMuas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NongDans",
                columns: table => new
                {
                    M_NongDan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Ten_NongDan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email_NongDan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT_NongDan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChi_NongDan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NongDans", x => x.M_NongDan);
                    table.ForeignKey(
                        name: "FK_NongDans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPhamGiamGias",
                columns: table => new
                {
                    M_LoaiSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_GiamGia = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPhamGiamGias", x => new { x.M_LoaiSP, x.M_GiamGia });
                    table.ForeignKey(
                        name: "FK_LoaiSanPhamGiamGias_GiamGias_M_GiamGia",
                        column: x => x.M_GiamGia,
                        principalTable: "GiamGias",
                        principalColumn: "M_GiamGia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoaiSanPhamGiamGias_LoaiSanPhams_M_LoaiSP",
                        column: x => x.M_LoaiSP,
                        principalTable: "LoaiSanPhams",
                        principalColumn: "M_LoaiSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    M_SanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_LoaiSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_DonViTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_KhoLuuTru = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<long>(type: "bigint", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnhSanPham = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.M_SanPham);
                    table.ForeignKey(
                        name: "FK_SanPhams_DonViTinhs_M_DonViTinh",
                        column: x => x.M_DonViTinh,
                        principalTable: "DonViTinhs",
                        principalColumn: "M_DonViTinh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhams_KhoLuuTrus_M_KhoLuuTru",
                        column: x => x.M_KhoLuuTru,
                        principalTable: "KhoLuuTrus",
                        principalColumn: "M_KhoLuuTru",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhams_LoaiSanPhams_M_LoaiSP",
                        column: x => x.M_LoaiSP,
                        principalTable: "LoaiSanPhams",
                        principalColumn: "M_LoaiSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    M_DonHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayGiao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    M_VanDon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_PhuongThuc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.M_DonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_PhuongThucThanhToans_M_PhuongThuc",
                        column: x => x.M_PhuongThuc,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "M_PhuongThuc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangs_VanChuyens_M_VanDon",
                        column: x => x.M_VanDon,
                        principalTable: "VanChuyens",
                        principalColumn: "M_VanDon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToanNongDans",
                columns: table => new
                {
                    M_ThanhToanNongDan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    M_NongDan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToanNongDans", x => x.M_ThanhToanNongDan);
                    table.ForeignKey(
                        name: "FK_ThanhToanNongDans_NongDans_M_NongDan",
                        column: x => x.M_NongDan,
                        principalTable: "NongDans",
                        principalColumn: "M_NongDan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuCauThuGoms",
                columns: table => new
                {
                    M_YeuCau = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayYeuCau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    M_NongDan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauThuGoms", x => x.M_YeuCau);
                    table.ForeignKey(
                        name: "FK_YeuCauThuGoms_NongDans_M_NongDan",
                        column: x => x.M_NongDan,
                        principalTable: "NongDans",
                        principalColumn: "M_NongDan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDanhGias",
                columns: table => new
                {
                    M_NguoiMua = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_SanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MucDoHaiLong = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MoTa_DanhGia = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDanhGias", x => new { x.M_NguoiMua, x.M_SanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietDanhGias_NguoiMuas_M_NguoiMua",
                        column: x => x.M_NguoiMua,
                        principalTable: "NguoiMuas",
                        principalColumn: "M_NguoiMua",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDanhGias_SanPhams_M_SanPham",
                        column: x => x.M_SanPham,
                        principalTable: "SanPhams",
                        principalColumn: "M_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhamGiamGias",
                columns: table => new
                {
                    M_SanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_GiamGia = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamGiamGias", x => new { x.M_SanPham, x.M_GiamGia });
                    table.ForeignKey(
                        name: "FK_SanPhamGiamGias_GiamGias_M_GiamGia",
                        column: x => x.M_GiamGia,
                        principalTable: "GiamGias",
                        principalColumn: "M_GiamGia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamGiamGias_SanPhams_M_SanPham",
                        column: x => x.M_SanPham,
                        principalTable: "SanPhams",
                        principalColumn: "M_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDatHangs",
                columns: table => new
                {
                    M_SanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_DonHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GiaDatHang = table.Column<long>(type: "bigint", nullable: false),
                    TongTien = table.Column<long>(type: "bigint", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    M_NguoiMua = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDatHangs", x => new { x.M_SanPham, x.M_DonHang });
                    table.ForeignKey(
                        name: "FK_ChiTietDatHangs_DonHangs_M_DonHang",
                        column: x => x.M_DonHang,
                        principalTable: "DonHangs",
                        principalColumn: "M_DonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDatHangs_NguoiMuas_M_NguoiMua",
                        column: x => x.M_NguoiMua,
                        principalTable: "NguoiMuas",
                        principalColumn: "M_NguoiMua",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDatHangs_SanPhams_M_SanPham",
                        column: x => x.M_SanPham,
                        principalTable: "SanPhams",
                        principalColumn: "M_SanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoanTras",
                columns: table => new
                {
                    M_HoanTra = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_DonHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TrangThaiSanPham = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    SoTienHoan = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoanTras", x => new { x.M_HoanTra, x.M_DonHang });
                    table.ForeignKey(
                        name: "FK_ChiTietHoanTras_DonHangs_M_DonHang",
                        column: x => x.M_DonHang,
                        principalTable: "DonHangs",
                        principalColumn: "M_DonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoanTras_HoanTras_M_HoanTra",
                        column: x => x.M_HoanTra,
                        principalTable: "HoanTras",
                        principalColumn: "M_HoanTra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuanLyNhaps",
                columns: table => new
                {
                    M_QuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    YeuCau = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    M_HoanTra = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_DonHang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLyNhaps", x => x.M_QuanLy);
                    table.ForeignKey(
                        name: "FK_QuanLyNhaps_DonHangs_M_DonHang",
                        column: x => x.M_DonHang,
                        principalTable: "DonHangs",
                        principalColumn: "M_DonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuanLyNhaps_HoanTras_M_HoanTra",
                        column: x => x.M_HoanTra,
                        principalTable: "HoanTras",
                        principalColumn: "M_HoanTra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuanLyNhaps_QuanLys_M_QuanLy",
                        column: x => x.M_QuanLy,
                        principalTable: "QuanLys",
                        principalColumn: "M_QuanLy",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietThuGoms",
                columns: table => new
                {
                    M_YeuCau = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_NongDan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_DonViTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    M_QuanLy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    M_SanPham = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietThuGoms", x => new { x.M_YeuCau, x.M_NongDan });
                    table.ForeignKey(
                        name: "FK_ChiTietThuGoms_DonViTinhs_M_DonViTinh",
                        column: x => x.M_DonViTinh,
                        principalTable: "DonViTinhs",
                        principalColumn: "M_DonViTinh",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietThuGoms_NongDans_M_NongDan",
                        column: x => x.M_NongDan,
                        principalTable: "NongDans",
                        principalColumn: "M_NongDan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietThuGoms_QuanLys_M_QuanLy",
                        column: x => x.M_QuanLy,
                        principalTable: "QuanLys",
                        principalColumn: "M_QuanLy",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietThuGoms_SanPhams_M_SanPham",
                        column: x => x.M_SanPham,
                        principalTable: "SanPhams",
                        principalColumn: "M_SanPham",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietThuGoms_YeuCauThuGoms_M_YeuCau",
                        column: x => x.M_YeuCau,
                        principalTable: "YeuCauThuGoms",
                        principalColumn: "M_YeuCau",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDanhGias_M_SanPham",
                table: "ChiTietDanhGias",
                column: "M_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatHangs_M_DonHang",
                table: "ChiTietDatHangs",
                column: "M_DonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatHangs_M_NguoiMua",
                table: "ChiTietDatHangs",
                column: "M_NguoiMua");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoanTras_M_DonHang",
                table: "ChiTietHoanTras",
                column: "M_DonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_M_DonViTinh",
                table: "ChiTietThuGoms",
                column: "M_DonViTinh");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_M_NongDan",
                table: "ChiTietThuGoms",
                column: "M_NongDan");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_M_QuanLy",
                table: "ChiTietThuGoms",
                column: "M_QuanLy");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuGoms_M_SanPham",
                table: "ChiTietThuGoms",
                column: "M_SanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_M_PhuongThuc",
                table: "DonHangs",
                column: "M_PhuongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_M_VanDon",
                table: "DonHangs",
                column: "M_VanDon");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiSanPhamGiamGias_M_GiamGia",
                table: "LoaiSanPhamGiamGias",
                column: "M_GiamGia");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiMuas_UserId",
                table: "NguoiMuas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NongDans_UserId",
                table: "NongDans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLyNhaps_M_DonHang",
                table: "QuanLyNhaps",
                column: "M_DonHang");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLyNhaps_M_HoanTra",
                table: "QuanLyNhaps",
                column: "M_HoanTra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamGiamGias_M_GiamGia",
                table: "SanPhamGiamGias",
                column: "M_GiamGia");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_M_DonViTinh",
                table: "SanPhams",
                column: "M_DonViTinh");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_M_KhoLuuTru",
                table: "SanPhams",
                column: "M_KhoLuuTru");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_M_LoaiSP",
                table: "SanPhams",
                column: "M_LoaiSP");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToanNongDans_M_NongDan",
                table: "ThanhToanNongDans",
                column: "M_NongDan");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauThuGoms_M_NongDan",
                table: "YeuCauThuGoms",
                column: "M_NongDan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChiTietDanhGias");

            migrationBuilder.DropTable(
                name: "ChiTietDatHangs");

            migrationBuilder.DropTable(
                name: "ChiTietHoanTras");

            migrationBuilder.DropTable(
                name: "ChiTietThuGoms");

            migrationBuilder.DropTable(
                name: "LoaiSanPhamGiamGias");

            migrationBuilder.DropTable(
                name: "QuanLyNhaps");

            migrationBuilder.DropTable(
                name: "SanPhamGiamGias");

            migrationBuilder.DropTable(
                name: "ThanhToanNongDans");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "NguoiMuas");

            migrationBuilder.DropTable(
                name: "YeuCauThuGoms");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "HoanTras");

            migrationBuilder.DropTable(
                name: "QuanLys");

            migrationBuilder.DropTable(
                name: "GiamGias");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "NongDans");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "VanChuyens");

            migrationBuilder.DropTable(
                name: "DonViTinhs");

            migrationBuilder.DropTable(
                name: "KhoLuuTrus");

            migrationBuilder.DropTable(
                name: "LoaiSanPhams");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
