// DACS/Models/ViewModels/OrderSummaryViewModel.cs
namespace DACS.Models.ViewModels
{
    public class OrderSummaryViewModel
    {
        public string OrderId { get; set; } // Hoặc kiểu int tùy vào model DonHang
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Trạng thái đơn hàng
        public decimal TotalAmount { get; set; } // Tổng tiền (tùy chọn)
        // Thêm các trường khác nếu cần hiển thị
    }
}