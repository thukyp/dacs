using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    public class NongDan // Farmer
    {
        [Key]
        [StringLength(10)]
        public string M_NongDan { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten_NongDan { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email_NongDan { get; set; }

        [Required]
        [StringLength(20)] // Tăng độ dài SĐT
        public string SDT_NongDan { get; set; }

        [Required]
        [StringLength(255)]
        public string DiaChi_NongDan { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        // Navigation Properties
        public virtual ICollection<ThanhToanNongDan> ThanhToanNongDans { get; set; } = new List<ThanhToanNongDan>();
        public virtual ICollection<ChiTietThuGom> ChiTietThuGoms { get; set; } = new List<ChiTietThuGom>();
        public virtual ICollection<YeuCauThuGom> YeuCauThuGoms { get; set; } = new List<YeuCauThuGom>(); // Cần FK trong YeuCauThuGom

        [Required] // <= Dòng này vẫn đang bị thiếu trong code của bạn cho UserId
        public string? UserId { get; set; } // <= Dấu ? này cần được BỎ ĐI ở bước cuối

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
