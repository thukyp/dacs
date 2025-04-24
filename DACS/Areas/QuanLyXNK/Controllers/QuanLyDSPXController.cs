using DACS.Models;
using DACS.Models.ViewModels;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLyXNK.Controllers
{
    [Area("QuanLyXNK")] // Chỉ định Area
    [Authorize(Roles = "Owner,QuanLyXNK")] // Ví dụ phân quyền: Chỉ Admin hoặc người có role QuanLyXNK mới vào được

    public class QuanLyDSPXController : Controller
    {
        private readonly IPhieuXuatRepository _phieuXuatRepo;
        private readonly ILogger<QuanLyDSPXController> _logger; // Inject logger

        public QuanLyDSPXController(IPhieuXuatRepository phieuXuatRepo, ILogger<QuanLyDSPXController> logger)
        {
            _phieuXuatRepo = phieuXuatRepo;
            _logger = logger;
        }

        // Action GET để hiển thị form tạo phiếu
        public IActionResult Create()
        {
            // Chuẩn bị dữ liệu cho dropdown (Kho hàng, Sản phẩm, Đơn vị tính...)
            // var viewModel = new CreatePhieuXuatViewModel { ... };
            return View();
        }

        // Action POST để xử lý tạo phiếu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePhieuXuatViewModel viewModel) // ViewModel chứa thông tin phiếu và chi tiết
        {
            if (!ModelState.IsValid)
            {
                // Chuẩn bị lại dropdown nếu cần
                return View(viewModel);
            }

            // Map từ ViewModel sang Model PhieuXuat và ChiTietPhieuXuat
            var phieuXuat = new PhieuXuat
            {
                NgayXuat = viewModel.NgayXuat ?? DateTime.UtcNow, // Hoặc lấy từ form
                MaKho = viewModel.MaKho,
                LyDoXuat = viewModel.LyDoXuat,
                ChiTietPhieuXuats = viewModel.ChiTietItems.Select(ct => new ChiTietPhieuXuat
                {
                    M_LoaiSP = ct.MaSanPham,
                    M_DonViTinh = ct.MaDonViTinh,
                    SoLuong = ct.SoLuong,
                    // DonGia = ct.DonGia // Nếu có
                }).ToList()
            };

            try
            {
                await _phieuXuatRepo.AddPhieuXuatAsync(phieuXuat);
                TempData["SuccessMessage"] = "Tạo phiếu xuất thành công!";
                return RedirectToAction(nameof(Index)); // Hoặc đến trang danh sách phiếu xuất
            }
            catch (InvalidOperationException ex) // Bắt lỗi cụ thể (ví dụ: thiếu tồn kho)
            {
                _logger.LogWarning("Lỗi khi tạo phiếu xuất: {ErrorMessage}", ex.Message);
                ModelState.AddModelError("", ex.Message); // Hiển thị lỗi cho người dùng
                                                          // Chuẩn bị lại dropdown nếu cần
                return View(viewModel);
            }
            catch (Exception ex) // Bắt các lỗi khác
            {
                _logger.LogError(ex, "Lỗi không xác định khi tạo phiếu xuất.");
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi tạo phiếu xuất.");
                // Chuẩn bị lại dropdown nếu cần
                return View(viewModel);
            }
        }
    }
}
