using System.ComponentModel.DataAnnotations;

namespace DACS.Models.ViewModels
{
    public class EditNguoiMuaProfile
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        [Display(Name = "Họ và Tên")]
        public string? Ten_NguoiMua { get; set; } // Sử dụng string? để tránh cảnh báo nullability nếu dùng .NET 6+

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email liên hệ")]
        public string? Email_NguoiMua { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Số điện thoại không hợp lệ.")] // Regex cơ bản cho SĐT
        [Display(Name = "Số điện thoại")]
        public string? SDT_NguoiMua { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi_NguoiMua { get; set; }

        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        // Thuộc tính này chỉ để hiển thị ảnh hiện tại, không cần validation
        [Display(Name = "Ảnh đại diện hiện tại")]
        public string? AvatarUrl { get; set; }

        // Thuộc tính để nhận file ảnh mới upload
        [Display(Name = "Chọn ảnh đại diện mới")]
        public IFormFile? ProfileImageFile { get; set; } // Dùng IFormFile để nhận file upload
    }
}
