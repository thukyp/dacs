using System.ComponentModel.DataAnnotations;

namespace Đồ_án_cs.Models
{
    public class NguoiMua // Buyer
    {
        [Key]
        [StringLength(10)]
        public string M_NguoiMua { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten_NguoiMua { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email_NguoiMua { get; set; }

        [Required]
        [StringLength(20)] // Tăng độ dài SĐT
        public string SDT_NguoiMua { get; set; }

        [Required]
        [StringLength(255)]
        public string DiaChi_NguoiMua { get; set; }

        // Navigation Properties
        public virtual ICollection<ChiTietDatHang> ChiTietDatHangs { get; set; } = new List<ChiTietDatHang>();
        public virtual ICollection<ChiTietDanhGia> ChiTietDanhGias { get; set; } = new List<ChiTietDanhGia>();

    }
}
