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
        public string M_KhachHang { get; set; } // PFK

        [Required]
        public int SoLuong { get; set; } // Số lượng thu gom

        [Display(Name = "Cồng Kềnh")]
        public bool DacTinh_CongKenh { get; set; } = false; // Map từ charBulky

        [Display(Name = "Ẩm/Ướt (Dễ hỏng)")]
        public bool DacTinh_AmUot { get; set; } = false; // Map từ charWet

        [Display(Name = "Khô (Dễ cháy)")]
        public bool DacTinh_Kho { get; set; } = false; // Map từ charDry

        [Display(Name = "Độ Ẩm Cao (>20%)")]
        public bool DacTinh_DoAmCao { get; set; } = false; // Map từ charMoisture

        [Display(Name = "Nhiều Tạp Chất")]
        public bool DacTinh_TapChat { get; set; } = false; // Map từ charImpure

        [Display(Name = "Đã Qua Xử Lý")]
        public bool DacTinh_DaXuLy { get; set; } = false; // Map từ charProcessed

        // --- THÊM: Danh sách hình ảnh ---
        [Display(Name = "Hình Ảnh")]
        public string? DanhSachHinhAnh { get; set; }

        // M_QuanLy FK mới
        [Required]
        [StringLength(10)]
        public string? M_QuanLy { get; set; } // FK

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

        [ForeignKey("M_KhachHang")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("M_QuanLy")]
        public virtual QuanLy? QuanLy { get; set; }
    }

}
