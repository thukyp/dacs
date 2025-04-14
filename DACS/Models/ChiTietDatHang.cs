using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Đồ_án_cs.Models
{
    public class ChiTietDatHang // Order Detail
    {
        [Required]
        [StringLength(10)]
        public string M_SanPham { get; set; } // PFK

        [Required]
        [StringLength(10)]
        public string M_DonHang { get; set; } // PFK

        [Required]
        // Nên dùng Decimal
        public long GiaDatHang { get; set; }

        [Required]
        // Nên dùng Decimal
        public long TongTien { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [StringLength(10)]
        // CẢNH BÁO: Cột này có vẻ thừa, nên bỏ đi.
        public string M_NguoiMua { get; set; } // FK?

        // Navigation Properties
        [ForeignKey("M_SanPham")]
        public virtual SanPham SanPham { get; set; }

        [ForeignKey("M_DonHang")]
        public virtual DonHang DonHang { get; set; }

        [ForeignKey("M_NguoiMua")]
        public virtual NguoiMua NguoiMua { get; set; } // Liên kết này có cần thiết?

    }
}
