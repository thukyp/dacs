using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class ChiTietThuGom // Collection Detail
    {
        [Required]
        [StringLength(10)]
        public string M_YeuCau { get; set; } // PFK

        [Required]
        [StringLength(10)]
        public string M_DonViTinh { get; set; } // FK

        [Required]
        [StringLength(10)]
        // CẢNH BÁO: Có thể thừa? PK nên là (M_YeuCau, M_SanPham)?
        public string M_NongDan { get; set; } // PFK

        [Required]
        public int SoLuong { get; set; } // Số lượng thu gom

        // M_QuanLy FK mới
        [Required]
        [StringLength(10)]
        public string M_QuanLy { get; set; } // FK

        // --- Thêm Khóa Ngoại và Navigation Property đến SanPham ---
        // (Cần thêm cột M_SanPham vào bảng ChiTietThuGom trong CSDL của bạn nếu chưa có)
        [Required(ErrorMessage = "Cần chỉ định sản phẩm được thu gom")]
        [StringLength(10)]
        public string M_SanPham { get; set; } // FK to SanPham - Đây là FK gây lỗi cascade

        [ForeignKey("M_SanPham")]
        public virtual SanPham SanPham { get; set; }
        // --- Hết phần thêm ---


        // Navigation Properties khác đã có
        [ForeignKey("M_YeuCau")]
        public virtual YeuCauThuGom YeuCauThuGom { get; set; }

        [ForeignKey("M_DonViTinh")]
        public virtual DonViTinh DonViTinh { get; set; }

        [ForeignKey("M_NongDan")]
        public virtual NongDan NongDan { get; set; }

        [ForeignKey("M_QuanLy")]
        public virtual QuanLy QuanLy { get; set; }
    }

}
