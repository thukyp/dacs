using System.ComponentModel.DataAnnotations;

namespace DACS.Models.ViewModels
{
    public class ThuGomViewModel
    {
        // --- Thông tin người cung cấp ---
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và Tên")]
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string? SupplierPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố.")]
        [Display(Name = "Tỉnh/Thành phố")]
        public string? SupplierProvince { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện.")]
        [Display(Name = "Quận/Huyện")]
        public string? SupplierDistrict { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Xã/Phường.")]
        [Display(Name = "Xã/Phường")]
        public string? SupplierWard { get; set; }

        [Display(Name = "Số nhà, Đường/Ấp/Thôn")]
        public string? SupplierStreet { get; set; } // Không bắt buộc trong HTML

        [Required(ErrorMessage = "Vui lòng chọn thời gian sẵn sàng thu gom.")]
        [Display(Name = "Thời gian sẵn sàng thu gom")]
        public DateTime? PickupReadyTime { get; set; }

        [Display(Name = "Ghi chú thêm")]
        public string? SupplierNotes { get; set; }

        // --- Thông tin phụ phẩm ---
        [Required(ErrorMessage = "Vui lòng chọn loại phụ phẩm.")]
        [Display(Name = "Loại phụ phẩm")]
        public string? ByproductType { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        public string? ByproductDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng ước tính.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        [Display(Name = "Số lượng ước tính")]
        public int? ByproductQuantity { get; set; } // Dùng int? hoặc double? tùy theo yêu cầu

        [Required(ErrorMessage = "Vui lòng chọn đơn vị tính.")]
        [Display(Name = "Đơn vị tính")]
        public string? ByproductUnit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị mong muốn không được âm.")]
        [Display(Name = "Giá trị mong muốn (Nếu có)")]
        public decimal? ByproductValue { get; set; } // Dùng decimal cho tiền tệ

        // Đặc tính phụ phẩm (bool vì là checkbox)
        [Display(Name = "Cồng kềnh")]
        public bool CharBulky { get; set; }
        [Display(Name = "Ẩm / Ướt")]
        public bool CharWet { get; set; }
        [Display(Name = "Khô")]
        public bool CharDry { get; set; }
        [Display(Name = "Độ ẩm cao (>20%)")]
        public bool CharMoisture { get; set; }
        [Display(Name = "Nhiều tạp chất")]
        public bool CharImpure { get; set; }
        [Display(Name = "Đã qua xử lý")]
        public bool CharProcessed { get; set; }

        [Display(Name = "Hình ảnh (Nếu có)")]
        public List<IFormFile>? ByproductImages { get; set; } // List để cho phép nhiều file

        // --- Điều khoản ---
        [Required(ErrorMessage = "Bạn phải đồng ý với Điều khoản & Quy định.")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý với Điều khoản & Quy định.")]
        [Display(Name = "Đồng ý điều khoản")]
        public bool TermsCheck { get; set; }
    }
}
