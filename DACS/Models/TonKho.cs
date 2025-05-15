using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class TonKho
    {
        [Key]
        public int Id { get; set; } // Khóa chính tự tăng

        [Required]
        [Display(Name = "Kho hàng")]
        [StringLength(10)]
        public string MaKho { get; set; } // FK đến KhoHang
        [ForeignKey("MaKho")]
        public virtual KhoHang KhoHang { get; set; }

        [Required]
        [Display(Name = "Sản phẩm")]
        [StringLength(10)]
        public string M_LoaiSP { get; set; } // FK đến SanPham
        [ForeignKey("M_LoaiSP")]
        public virtual LoaiSanPham LoaiSanPham { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public long SoLuong { get; set; } // Số lượng tồn

        [DataType(DataType.Date)]
        [Display(Name = "Ngày nhập")]
        public DateTime NgayNhapKho { get; set; } = DateTime.UtcNow.Date;

        [DataType(DataType.Date)]
        [Display(Name = "Hạn sử dụng")]
        public DateTime? HanSuDung { get; set; }

        [StringLength(50)]
        [Display(Name = "Số lô")]
        public string? SoLo { get; set; }

        // Có thể thêm Đơn vị tính nếu cần (FK đến DonViTinh)

        public string M_DonViTinh { get; set; } 

        [ForeignKey("MaDonViTinh")] 
        public virtual DonViTinh DonViTinh { get; set; }
    }
}
