using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Đồ_án_cs.Models
{
    public class DonHang // Order
    {
        [Key]
        [StringLength(10)]
        public string M_DonHang { get; set; }

        [Required]
        public DateTime NgayDat { get; set; } = DateTime.UtcNow;

        // Nên cho phép NULL ban đầu
        public DateTime? NgayGiao { get; set; } // DateTime?

        [Required]
        [StringLength(50)] // Tăng độ dài trạng thái
        public string TrangThai { get; set; }

        [Required]
        [StringLength(10)]
        public string M_VanDon { get; set; } // FK

        [Required]
        [StringLength(10)]
        public string M_PhuongThuc { get; set; } // FK

        // Navigation Properties
        [ForeignKey("M_VanDon")]
        public virtual VanChuyen VanChuyen { get; set; }

        [ForeignKey("M_PhuongThuc")]
        public virtual PhuongThucThanhToan PhuongThucThanhToan { get; set; }

        public virtual ICollection<ChiTietDatHang> ChiTietDatHangs { get; set; } = new List<ChiTietDatHang>();
        public virtual ICollection<ChiTietHoanTra> ChiTietHoanTras { get; set; } = new List<ChiTietHoanTra>();
        public virtual ICollection<QuanLyNhap> QuanLyNhaps { get; set; } = new List<QuanLyNhap>();

    }
}
