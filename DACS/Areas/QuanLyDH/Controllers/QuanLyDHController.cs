
using DACS.Models;
using DACS.Areas.Owner.Models; // ViewModel của bạn
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization; // Cho định dạng tiền tệ

namespace DACS.Areas.QuanLyDH.Controllers
{
    [Area("QuanLyDH")]
    [Authorize(Roles = SD.Role_Owner + "," + SD.Role_QuanLyDH)] // Thêm phân quyền
    public class QuanLyDHController : Controller
    {
        private readonly ApplicationDbContext _context;

        // --- ĐỊNH NGHĨA CÁC CHUỖI TRẠNG THÁI ĐƠN HÀNG TRONG DATABASE ---
        private const string StatusPendingDbValue = "Chờ xác nhận";
        private const string StatusConfirmedDbValue = "Đã xác nhận";
        private const string StatusProcessingDbValue = "Đang xử lý";
        private const string StatusShippingDbValue = "Đang giao hàng";
        private const string StatusCompletedDbValue = "Hoàn thành";
        private const string StatusCancelledDbValue = "Đã hủy";
        // --- KẾT THÚC ĐỊNH NGHĨA ---

        public QuanLyDHController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuanLyDH/QuanLyDonHang
        public async Task<IActionResult> Index(
            string searchInput,         // name="searchInput"
            DateTime? dateFromFilter,   // name="dateFromFilter"
            DateTime? dateToFilter,     // name="dateToFilter"
            string statusFilterSelect,  // name="statusFilterSelect"
            string paymentStatusFilter, // name="paymentStatusFilter" (sẽ không dùng để query DB nếu DonHang model ko có)
            int page = 1)
        {
            ViewData["Title"] = "Quản Lý Đơn Hàng";
            int pageSize = 5; // Số lượng item mỗi trang, ví dụ

            var viewModel = new Owner.Models.OrderManagementViewModel
            {
                SearchInput = searchInput,
                DateFromFilter = dateFromFilter,
                DateToFilter = dateToFilter,
                SelectedOrderStatus = statusFilterSelect,
                SelectedPaymentStatus = paymentStatusFilter, // Giữ lại để điền vào dropdown
                OrderStatusOptions = GetOrderStatusOptions(statusFilterSelect),
                PaymentStatusOptions = GetPaymentStatusOptions(paymentStatusFilter), // Tạo options cho TT TT
                CurrentPage = page
            };

            IQueryable<DonHang> donHangQuery = _context.DonHangs
                                               .Include(dh => dh.KhachHang)
                                               .Include(dh => dh.PhuongThucThanhToan)
                                               .OrderByDescending(dh => dh.NgayDat);

            if (!string.IsNullOrEmpty(searchInput))
            {
                string searchLower = searchInput.ToLower();
                donHangQuery = donHangQuery.Where(dh =>
                    dh.M_DonHang.ToLower().Contains(searchLower) ||
                    (dh.KhachHang != null && dh.KhachHang.Ten_KhachHang.ToLower().Contains(searchLower)) ||
                    (dh.KhachHang != null && dh.KhachHang.SDT_KhachHang.Contains(searchInput))
                );
            }
            if (dateFromFilter.HasValue)
            {
                donHangQuery = donHangQuery.Where(dh => dh.NgayDat.Date >= dateFromFilter.Value.Date);
            }
            if (dateToFilter.HasValue)
            {
                donHangQuery = donHangQuery.Where(dh => dh.NgayDat.Date <= dateToFilter.Value.Date);
            }
            if (!string.IsNullOrEmpty(statusFilterSelect))
            {
                string dbOrderStatus = MapFilterToDbOrderStatus(statusFilterSelect);
                if (!string.IsNullOrEmpty(dbOrderStatus))
                {
                    donHangQuery = donHangQuery.Where(dh => dh.TrangThai == dbOrderStatus);
                }
            }
            // Lọc theo paymentStatusFilter SẼ KHÔNG được thực hiện ở đây vì DonHang model không có
            // Nếu bạn muốn filter theo "Trạng thái TT" suy luận, bạn phải ToList() trước rồi filter trên list đó,
            // nhưng sẽ không hiệu quả cho pagination.

            var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

            viewModel.PendingConfirmationOrders = await _context.DonHangs.CountAsync(d => d.TrangThai == StatusPendingDbValue);
            viewModel.ProcessingOrShippingOrders = await _context.DonHangs.CountAsync(d => d.TrangThai == StatusConfirmedDbValue || d.TrangThai == StatusProcessingDbValue || d.TrangThai == StatusShippingDbValue);
            viewModel.CompletedOrdersThisMonth = await _context.DonHangs.CountAsync(d => d.TrangThai == StatusCompletedDbValue && d.NgayDat >= currentMonthStart && d.NgayDat <= currentMonthEnd);
            viewModel.CancelledOrdersThisMonth = await _context.DonHangs.CountAsync(d => d.TrangThai == StatusCancelledDbValue && d.NgayDat >= currentMonthStart && d.NgayDat <= currentMonthEnd);

            int totalItems = await donHangQuery.CountAsync();
            viewModel.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (viewModel.CurrentPage < 1) viewModel.CurrentPage = 1;
            if (viewModel.CurrentPage > viewModel.TotalPages && viewModel.TotalPages > 0) viewModel.CurrentPage = viewModel.TotalPages;

            var pagedDonHangs = await donHangQuery
                                      .Skip((viewModel.CurrentPage - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            viewModel.Orders = pagedDonHangs.Select(dh => new OrderViewModel
            {
                M_DonHang = dh.M_DonHang,
                CustomerName = dh.KhachHang?.Ten_KhachHang ?? "N/A",
                CustomerPhoneNumber = dh.KhachHang?.SDT_KhachHang ?? "N/A",
                OrderDate = dh.NgayDat,
                TotalAmount = (decimal)dh.TotalPrice,
                OrderStatusText = dh.TrangThai, // Lấy trực tiếp từ DB
                OrderStatusBadgeClass = GetOrderStatusBadgeClass(dh.TrangThai),
                PaymentStatusText = InferPaymentStatusText(dh), // Hàm suy luận TT Thanh toán
                PaymentStatusBadgeClass = InferPaymentStatusBadgeClass(InferPaymentStatusText(dh)) // Hàm suy luận badge TT TT
            }).ToList();

            return View(viewModel);
        }

        // --- Các hàm helper cho Trạng Thái Đơn Hàng (ĐH) ---
        private List<SelectListItem> GetOrderStatusOptions(string selectedValue)
        {
            return new List<SelectListItem> {
                new SelectListItem { Value = "", Text = "Trạng thái ĐH", Selected = string.IsNullOrEmpty(selectedValue) },
                new SelectListItem { Value = "pending", Text = "Chờ xác nhận", Selected = selectedValue == "pending" },
                new SelectListItem { Value = "confirmed", Text = "Đã xác nhận", Selected = selectedValue == "confirmed" },
                new SelectListItem { Value = "shipping", Text = "Đang giao", Selected = selectedValue == "shipping" },
                new SelectListItem { Value = "completed", Text = "Hoàn thành", Selected = selectedValue == "completed" },
                new SelectListItem { Value = "cancelled", Text = "Đã hủy", Selected = selectedValue == "cancelled" }
            };
        }
        private string MapFilterToDbOrderStatus(string filterStatus)
        { /* Giữ nguyên từ code trước */
            return filterStatus?.ToLower() switch
            {
                "pending" => StatusPendingDbValue,
                "confirmed" => StatusConfirmedDbValue,
                "shipping" => StatusShippingDbValue,
                "completed" => StatusCompletedDbValue,
                "cancelled" => StatusCancelledDbValue,
                _ => string.Empty,
            };
        }
        private string GetOrderStatusBadgeClass(string dbStatus)
        { /* Giữ nguyên từ code trước */
            if (dbStatus == StatusPendingDbValue) return "badge bg-warning text-dark";
            if (dbStatus == StatusConfirmedDbValue) return "badge bg-info text-dark";
            if (dbStatus == StatusProcessingDbValue) return "badge bg-primary";
            if (dbStatus == StatusShippingDbValue) return "badge bg-primary";
            if (dbStatus == StatusCompletedDbValue) return "badge bg-success";
            if (dbStatus == StatusCancelledDbValue) return "badge bg-danger";
            return "badge bg-secondary";
        }

        // --- Các hàm helper MỚI cho Trạng Thái Thanh Toán (TT) ---
        private List<SelectListItem> GetPaymentStatusOptions(string selectedValue)
        {
            // Các value "unpaid", "paid", "refunded" khớp với HTML
            return new List<SelectListItem> {
                new SelectListItem { Value = "", Text = "Trạng thái TT", Selected = string.IsNullOrEmpty(selectedValue) },
                new SelectListItem { Value = "unpaid", Text = "Chưa thanh toán", Selected = selectedValue == "unpaid" },
                new SelectListItem { Value = "paid", Text = "Đã thanh toán", Selected = selectedValue == "paid" },
                new SelectListItem { Value = "refunded", Text = "Đã hoàn tiền", Selected = selectedValue == "refunded" }
            };
        }

        private string InferPaymentStatusText(DonHang dh)
        {
            // Logic suy luận rất cơ bản - bạn cần tùy chỉnh cho phù hợp
            // Ví dụ này dựa trên HTML sample data
            if (dh.TrangThai == StatusCancelledDbValue) return "Đã hoàn tiền";
            if (dh.TrangThai == StatusPendingDbValue) return "Chờ TT";
            if (dh.TrangThai == StatusConfirmedDbValue ||
                dh.TrangThai == StatusProcessingDbValue ||
                dh.TrangThai == StatusShippingDbValue ||
                dh.TrangThai == StatusCompletedDbValue)
            {
                // Giả sử nếu đã qua "Chờ xác nhận" thì là "Đã TT"
                // Điều này có thể không đúng với COD. Cần logic phức tạp hơn nếu có nhiều PTTT.
                return "Đã TT";
            }
            return "Chưa rõ"; // Mặc định
        }

        private string InferPaymentStatusBadgeClass(string paymentStatusText)
        {
            return paymentStatusText switch
            {
                "Chờ TT" => "badge bg-warning text-dark",
                "Đã TT" => "badge bg-success",
                "Đã hoàn tiền" => "badge bg-secondary", // Khớp với HTML sample, có thể là bg-danger nếu muốn
                _ => "badge bg-light text-dark",
            };
        }


        // --- Action Details và UpdateStatus --- (Giữ nguyên logic như code trước, chỉ đảm bảo dùng đúng hằng số chuỗi)
        public async Task<IActionResult> Details(string id)
        { /* ... code như trước ... */
            if (string.IsNullOrEmpty(id)) return NotFound();
            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.PhuongThucThanhToan)
                .Include(d => d.VanChuyen)
                .Include(d => d.ChiTietDatHangs).ThenInclude(ctdh => ctdh.SanPham)
                .FirstOrDefaultAsync(m => m.M_DonHang == id);
            if (donHang == null) return NotFound();
            ViewData["Title"] = $"Chi tiết đơn hàng {donHang.M_DonHang}";
            ViewData["OrderStatusBadgeClass"] = GetOrderStatusBadgeClass(donHang.TrangThai);
            // Suy luận và truyền PaymentStatus cho View Details nếu cần
            string paymentStatusText = InferPaymentStatusText(donHang);
            ViewData["PaymentStatusText"] = paymentStatusText;
            ViewData["PaymentStatusBadgeClass"] = InferPaymentStatusBadgeClass(paymentStatusText);
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(string id, string newStatusKey)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newStatusKey))
                return BadRequest("Thông tin không hợp lệ.");

            var donHang = await _context.DonHangs.FirstOrDefaultAsync(m => m.M_DonHang == id);
            if (donHang == null)
                return NotFound($"Không tìm thấy đơn hàng {id}.");
            
            string actualNewStatusInDb = "";
            string successMessagePart = "";

            switch (newStatusKey.ToLower())
            {
                case "confirm":
                    if (donHang.TrangThai == StatusPendingDbValue)
                    {
                        actualNewStatusInDb = StatusConfirmedDbValue;
                        successMessagePart = "xác nhận.";
                    }
                    break;
                case "ship":
                    if (donHang.TrangThai == StatusConfirmedDbValue || donHang.TrangThai == StatusProcessingDbValue)
                    {
                        actualNewStatusInDb = StatusShippingDbValue;
                        successMessagePart = "chuyển sang đang giao.";
                    }
                    break;
                case "complete":
                    
                    if (donHang.TrangThai == StatusShippingDbValue)
                    {
                        actualNewStatusInDb = StatusCompletedDbValue;
                        successMessagePart = "hoàn thành.";
                    }
                    break;
                case "cancel":
                    if (donHang.TrangThai == StatusPendingDbValue || donHang.TrangThai == StatusConfirmedDbValue)
                    {
                        actualNewStatusInDb = StatusCancelledDbValue;
                        successMessagePart = "hủy.";
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(actualNewStatusInDb))
            {
                donHang.TrangThai = actualNewStatusInDb;
                _context.Update(donHang);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đơn hàng {donHang.M_DonHang} đã được {successMessagePart}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = $"Không thể cập nhật trạng thái cho đơn hàng {donHang.M_DonHang}. Chuyển đổi trạng thái không hợp lệ từ '{donHang.TrangThai}'.";
                return BadRequest("Chuyển đổi trạng thái không hợp lệ.");
            }
        }
    }
}