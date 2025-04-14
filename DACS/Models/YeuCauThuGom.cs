using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Đồ_án_cs.Models
{
    public class YeuCauThuGom // Collection Request
    {
        [Key]
        [StringLength(10)]
        public string M_YeuCau { get; set; }

        [Required]
        public DateTime NgayYeuCau { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)] // Tăng độ dài
        public string TrangThai { get; set; }

        // --- Missing FK to NongDan based on logic/ERD? ---
        [Required]
        [StringLength(10)]
        public string M_NongDan { get; set; }
        [ForeignKey("M_NongDan")]
        public virtual NongDan NongDan { get; set; }

        // Navigation Property
        public virtual ICollection<ChiTietThuGom> ChiTietThuGoms { get; set; } = new List<ChiTietThuGom>();

    }
}
