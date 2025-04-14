using System.ComponentModel.DataAnnotations;

namespace Đồ_án_cs.Models
{
    public class KhoLuuTru // Warehouse / Storage
    {
        [Key]
        [StringLength(10)]
        public string M_KhoLuuTru { get; set; }

        [Required]
        // Cân nhắc dùng Decimal nếu số lượng có thể lẻ
        public long SoLuongTon { get; set; } // Bigint

        [Required]
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime HanSuDung { get; set; }

        // Navigation Property
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
