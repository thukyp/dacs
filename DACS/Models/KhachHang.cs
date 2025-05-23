﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; // Thêm nếu ApplicationUser ở namespace khác
using System.Collections.Generic; // Thêm cho ICollection

namespace DACS.Models
{
    public class KhachHang
    {
        [Key]
        [StringLength(10)]
        public string M_KhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        [StringLength(100)]
        public string Ten_KhachHang { get; set; }

        // Email liên hệ - Cho phép null nếu không bắt buộc phải có khi tạo/sửa hồ sơ
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email liên hệ")]
        public string? Email_KhachHang { get; set; } // <<< Cho phép null

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [StringLength(20)]
        [Phone]
        public string SDT_KhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố.")]
        [StringLength(10)] // Hoặc int
        [Display(Name = "Tỉnh/Thành phố")]
        public string? MaTinh { get; set; } // <<< Khóa ngoại đến TinhThanhPho

        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện.")]
        [StringLength(10)] // Hoặc int
        [Display(Name = "Quận/Huyện")]
        public string? MaQuan { get; set; } // <<< Khóa ngoại đến QuanHuyen

        [Required(ErrorMessage = "Vui lòng chọn Xã/Phường.")]
        [StringLength(10)] // Hoặc int
        [Display(Name = "Xã/Phường")]
        public string? MaXa { get; set; } // <<< Khóa ngoại đến XaPhuong
        // -------------------------------------------

        // --- Giữ lại trường địa chỉ chi tiết ---
        [StringLength(200)]
        [Display(Name = "Số nhà, Đường/Ấp/Thôn")]
        public string? DiaChi_DuongApThon { get; set; }
        // ---------------------------------

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        // --- SỬA UserId: Bỏ nullable, giữ Required ---
        [Required]
        public string UserId { get; set; } // <<< Bỏ dấu ?
        // ------------------------------------------

        // --- Navigation Properties ---
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; } // OK

        // --- Navigation đến các bảng địa chỉ ---
        [ForeignKey("MaTinh")] // Đảm bảo ForeignKey trỏ đúng vào thuộc tính MaTinh ở trên
        public virtual TinhThanhPho? TinhThanhPho { get; set; } // Cho phép null nếu DB cho phép

        [ForeignKey("MaQuan")] // Đảm bảo ForeignKey trỏ đúng vào thuộc tính MaQuan ở trên
        public virtual QuanHuyen? QuanHuyen { get; set; } // Cho phép null nếu DB cho phép

        [ForeignKey("MaXa")] // Đảm bảo ForeignKey trỏ đúng vào thuộc tính MaXa ở trên
        public virtual XaPhuong? XaPhuong { get; set; } // Cho phép null nếu DB cho phép
        // --- Kết thúc Navigation địa chỉ ---

        public virtual ICollection<ChiTietDatHang> ChiTietDatHangs { get; set; } = new List<ChiTietDatHang>();
        public virtual ICollection<ChiTietDanhGia> ChiTietDanhGias { get; set; } = new List<ChiTietDanhGia>();
        public virtual ICollection<YeuCauThuGom> YeuCauThuGoms { get; set; } = new List<YeuCauThuGom>();
        // -----------------------------
    }
}