namespace Đồ_án_cs.Models
{
    public class ChiTietGiamGia
    {
        public string M_NguoiMua { get; set; } // Varchar(10) PFK
        public string M_SanPham { get; set; } // Varchar(10) PFK
        public string? MucDoHaiLong { get; set; } // Varchar(10) NULL - Nên là int?
        public string? MoTa_DanhGia { get; set; } // Ntext NULL

        // Navigation Properties
        public virtual NguoiMua NguoiMua { get; set; }
        public virtual SanPham SanPham { get; set; }

    }
}
