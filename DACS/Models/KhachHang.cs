using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    public class KhachHang // Buyer
    {
        [Key]
        [StringLength(10)]
        public string M_KhachHang    { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten_KhachHang { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email_KhachHang { get; set; }

        [Required]
        [StringLength(20)] // Tăng độ dài SĐT
        public string SDT_KhachHang { get; set; }

        [Required]
        [StringLength(255)]
        public string DiaChi_KhachHang { get; set; }

        [StringLength(10)] 
        public string? Gender { get; set; } 

        [StringLength(255)] 
        public string? AvatarUrl { get; set; }

        // Navigation Properties
        public virtual ICollection<ChiTietDatHang> ChiTietDatHangs { get; set; } = new List<ChiTietDatHang>();
        public virtual ICollection<ChiTietDanhGia> ChiTietDanhGias { get; set; } = new List<ChiTietDanhGia>();

        public virtual ICollection<YeuCauThuGom> YeuCauThuGoms { get; set; } = new List<YeuCauThuGom>();

        [Required]
        public string? UserId { get; set; } // <<< SỬA LẠI: Bỏ dấu ?

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

    }
}
