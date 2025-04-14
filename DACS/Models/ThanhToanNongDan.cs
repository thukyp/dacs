using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Đồ_án_cs.Models
{
    public class ThanhToanNongDan // Farmer Payment
    {
        [Key]
        [StringLength(10)]
        public string M_ThanhToanNongDan { get; set; }

        [Required]
        // Nên dùng Decimal
        public long SoTien { get; set; }

        [Required]
        public DateTime NgayThanhToan { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(10)]
        public string M_NongDan { get; set; } // FK

        // Navigation Property
        [ForeignKey("M_NongDan")]
        public virtual NongDan NongDan { get; set; }
    }
}
