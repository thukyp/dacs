using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    public class PhieuXuat
    {
        [Key]
        public int MaPhieuXuat { get; set; } // Hoặc kiểu string nếu mã phức tạp

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime NgayXuat { get; set; }

        public string? LyDoXuat { get; set; }
        // Các trường khác nếu cần: Người tạo, Kho xuất...

        [Required(ErrorMessage = "Vui lòng chọn kho xuất.")]
        [StringLength(10)]
        [Display(Name = "Kho xuất")]
        public string MaKho { get; set; } // FK đến KhoHang
        [ForeignKey("MaKho")]
        public virtual KhoHang? KhoHang { get; set; } // Thêm navigation property


        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();
    }
}