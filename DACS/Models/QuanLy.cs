using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class QuanLy
    {
        [Key]
        [StringLength(10)]
        public string M_QuanLy { get; set; }

        // Nên dùng bool (bit trong SQL)
        [StringLength(3)]
        public string? XacNhan { get; set; }

        // Navigation Property
        // Quan hệ 1-1 thì cần cấu hình Fluent API
        public virtual QuanLyNhap QuanLyNhap { get; set; }
        public virtual ICollection<ChiTietThuGom> ChiTietThuGoms { get; set; } = new List<ChiTietThuGom>();


    }
}
