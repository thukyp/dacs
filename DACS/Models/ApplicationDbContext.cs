using DACS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Constructor required for dependency injection
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<KhachHang> KhachHangs { get; set; }
    public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
    public DbSet<DonViTinh> DonViTinhs { get; set; }
    public DbSet<KhoLuuTru> KhoLuuTrus { get; set; }
    public DbSet<SanPham> SanPhams { get; set; }
    public DbSet<DonHang> DonHangs { get; set; }
    public DbSet<ChiTietDatHang> ChiTietDatHangs { get; set; }
    public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
    public DbSet<VanChuyen> VanChuyens { get; set; }
    public DbSet<ChiTietDanhGia> ChiTietDanhGias { get; set; }
    public DbSet<GiamGia> GiamGias { get; set; }
    public DbSet<SanPhamGiamGia> SanPhamGiamGias { get; set; } // Bảng nối Sản phẩm - Giảm giá
    public DbSet<LoaiSanPhamGiamGia> LoaiSanPhamGiamGias { get; set; } // Bảng nối Loại SP - Giảm giá
    public DbSet<YeuCauThuGom> YeuCauThuGoms { get; set; }
    public DbSet<ChiTietThuGom> ChiTietThuGoms { get; set; }
    public DbSet<HoanTra> HoanTras { get; set; }
    public DbSet<ChiTietHoanTra> ChiTietHoanTras { get; set; }
    public DbSet<Owner> Owner { get; set; }
    public DbSet<QuanLy> QuanLys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Rất quan trọng! Cấu hình các bảng Identity trước.

        // --- Cấu hình Khóa chính phức hợp ---
        builder.Entity<ChiTietDatHang>()
            .HasKey(cdh => new { cdh.M_SanPham, cdh.M_DonHang }); // Composite key

        builder.Entity<ChiTietDanhGia>()
            .HasKey(cdg => new { cdg.M_KhachHang, cdg.M_SanPham }); // Composite key for ReviewDetail // <<< CONFIGURATION IS PRESENT

        builder.Entity<ChiTietHoanTra>()
            .HasKey(ctht => new { ctht.M_HoanTra, ctht.M_DonHang }); // Composite key (Lưu ý: M_DonHang có thể không đủ chi tiết)

        builder.Entity<ChiTietThuGom>()
            .HasKey(ctg => new { ctg.M_YeuCau, ctg.M_KhachHang }); // Composite key (Lưu ý: M_NongDan có thể thừa)

        builder.Entity<SanPhamGiamGia>()
            .HasKey(spg => new { spg.M_SanPham, spg.M_GiamGia }); // Composite key

        builder.Entity<LoaiSanPhamGiamGia>()
            .HasKey(lsg => new { lsg.M_LoaiSP, lsg.M_GiamGia }); // Composite key

        builder.Entity<GiamGia>() // Configuration for GiamGia entity
           .Property(gg => gg.GiaTriDonHangToiThieu).HasColumnType("decimal(18, 2)");
        // MISSING CONFIGURATION FOR GiaTriDonHangToiThieu HERE <<<<<<

        builder.Entity<ChiTietHoanTra>()
           .Property(ctht => ctht.SoTienHoan)
           .HasColumnType("decimal(18, 2)");

        builder.Entity<ChiTietThuGom>(entity =>
        {
            // Cấu hình PK phức hợp (đã làm ở trên hoặc làm ở đây cũng được)
            // entity.HasKey(ctg => new { ctg.M_YeuCau, ctg.M_NongDan });

            // Quan hệ với YeuCauThuGom
            entity.HasOne(ctg => ctg.YeuCauThuGom)
                  .WithMany(yc => yc.ChiTietThuGoms)
                  .HasForeignKey(ctg => ctg.M_YeuCau)
                  .OnDelete(DeleteBehavior.Restrict); // Ngăn cascade delete

            // Quan hệ với DonViTinh
            entity.HasOne(ctg => ctg.DonViTinh)
                  .WithMany(dvt => dvt.ChiTietThuGoms)
                  .HasForeignKey(ctg => ctg.M_DonViTinh)
                  .OnDelete(DeleteBehavior.Restrict); // Ngăn cascade delete

            // Quan hệ với NongDan
            entity.HasOne(ctg => ctg.KhachHang)
                  .WithMany(nd => nd.ChiTietThuGoms)
                  .HasForeignKey(ctg => ctg.M_KhachHang)
                  .OnDelete(DeleteBehavior.Restrict); // Ngăn cascade delete

            // Quan hệ với QuanLy
            entity.HasOne(ctg => ctg.QuanLy)
                  .WithMany(ql => ql.ChiTietThuGoms) // Giả sử QuanLy có ICollection<ChiTietThuGom>
                  .HasForeignKey(ctg => ctg.M_QuanLy)
                  .OnDelete(DeleteBehavior.Restrict); // Ngăn cascade delete

            // Quan hệ với SanPham (Gây lỗi ban đầu)
            entity.HasOne(ctg => ctg.SanPham)
                  .WithMany(sp => sp.ChiTietThuGoms)
                  .HasForeignKey(ctg => ctg.M_SanPham)
                  .OnDelete(DeleteBehavior.Restrict); // QUAN TRỌNG: Ngăn cascade delete


        // Cấu hình User <-> KhachHang
        builder.Entity<KhachHang>()
            .HasOne(kh => kh.User)
            .WithOne() // Hoặc .WithOne(u => u.KhachHangProfile) nếu bạn thêm nav prop vào ApplicationUser
            .HasForeignKey<KhachHang>(kh => kh.UserId)
            .IsRequired();

        // Cấu hình KhachHang -> YeuCauThuGom (One-to-Many)
        // Giả sử YeuCauThuGom có khóa ngoại là M_KhachHang và navigation property là KhachHang
        builder.Entity<KhachHang>()
               .HasMany(kh => kh.YeuCauThuGoms)
               .WithOne(yctg => yctg.KhachHang) // Tên navigation property trong YeuCauThuGom
               .HasForeignKey(yctg => yctg.M_KhachHang); // Tên khóa ngoại trong YeuCauThuGom

        // Cấu hình KhachHang -> ChiTietDatHang (One-to-Many)
        builder.Entity<KhachHang>()
              .HasMany(kh => kh.ChiTietDatHangs)
              .WithOne(ctdh => ctdh.KhachHang) // Tên navigation property trong ChiTietDatHang
              .HasForeignKey(ctdh => ctdh.M_KhachHang); // Tên khóa ngoại trong ChiTietDatHang

    });
    }
}
