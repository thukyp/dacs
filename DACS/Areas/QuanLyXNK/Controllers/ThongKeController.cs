// File: Areas/QuanLyXNK/Controllers/ThongKeController.cs
using DACS.Models;
using DACS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DACS.Areas.QuanLyXNK.Controllers
{
    [Area("QuanLyXNK")]
    [Authorize(Roles = "Owner,QuanLyXNK")]
    public class ThongKeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThongKeController> _logger;

        public ThongKeController(ApplicationDbContext context, ILogger<ThongKeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: QuanLyXNK/ThongKe
        public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay)
        {
            _logger.LogInformation("Vào Thống kê Index - Từ: {TuNgay}, Đến: {DenNgay}", tuNgay, denNgay);

            // --- Xử lý khoảng thời gian mặc định ---
            DateTime startDate = tuNgay ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // Ngày đầu tháng hiện tại
            DateTime endDate = denNgay ?? DateTime.Today; // Ngày hiện tại
            DateTime endDateForQuery = endDate.AddDays(1); // Để bao gồm cả ngày kết thúc trong truy vấn (< endDateForQuery)

            var viewModel = new ThongKeNXViewModel
            {
                TuNgay = startDate,
                DenNgay = endDate,
                ThongKeItems = new List<ThongKeItemViewModel>()
            };

            try
            {
                // --- Lấy danh sách tất cả sản phẩm có liên quan (từ tồn kho, nhập, xuất) ---
                var productsInImports = await _context.ChiTietThuGoms
                                           .Include(ct => ct.YeuCauThuGom)
                                           .Where(ct => (ct.YeuCauThuGom.TrangThai == "Hoàn thành" || ct.YeuCauThuGom.TrangThai == "Thu gom thành công") &&
                                                        ct.YeuCauThuGom.ThoiGianHoanThanh >= startDate && ct.YeuCauThuGom.ThoiGianHoanThanh < endDateForQuery)
                                           .Select(ct => new { ct.M_LoaiSP, ct.M_DonViTinh })
                                           .Distinct()
                                           .ToListAsync();

                var productsInExports = await _context.ChiTietPhieuXuats // Giả định có bảng này
                                           .Include(ct => ct.PhieuXuat)
                                           .Where(ct => ct.PhieuXuat.NgayXuat >= startDate && ct.PhieuXuat.NgayXuat < endDateForQuery)
                                           .Select(ct => new { ct.M_LoaiSP, ct.M_DonViTinh })
                                           .Distinct()
                                           .ToListAsync();

                var productsInStock = await _context.TonKhos
                                        .Select(tk => new { tk.M_LoaiSP, tk.M_DonViTinh })
                                        .Distinct()
                                        .ToListAsync();

                // Kết hợp và lấy danh sách duy nhất (Mã SP, Mã ĐVT)
                var allRelevantProducts = productsInImports
                    .Union(productsInExports)
                    .Union(productsInStock)
                    .Distinct()
                    .ToList();


                if (!allRelevantProducts.Any())
                {
                    _logger.LogInformation("Không tìm thấy sản phẩm nào liên quan.");
                    return View(viewModel); // Trả về view rỗng nếu không có sản phẩm nào
                }


                // --- Lấy thông tin chi tiết Sản phẩm và Đơn vị tính ---
                var productCodes = allRelevantProducts.Select(p => p.M_LoaiSP).Distinct().ToList();
                var unitCodes = allRelevantProducts.Select(p => p.M_DonViTinh).Distinct().ToList();

                var productDetails = await _context.LoaiSanPhams
                                            .Where(lsp => productCodes.Contains(lsp.M_LoaiSP))
                                            .ToDictionaryAsync(lsp => lsp.M_LoaiSP, lsp => lsp.TenLoai);

                var unitDetails = await _context.DonViTinhs
                                        .Where(dvt => unitCodes.Contains(dvt.M_DonViTinh))
                                        .ToDictionaryAsync(dvt => dvt.M_DonViTinh, dvt => dvt.TenLoaiTinh);

                // --- Tính toán Nhập, Xuất, Tồn cho từng sản phẩm ---
                // Cách 1: Tối ưu hơn bằng Grouping (nên dùng nếu dữ liệu lớn)
                var importsGrouped = await _context.ChiTietThuGoms
                    .Include(ct => ct.YeuCauThuGom)
                    .Where(ct => (ct.YeuCauThuGom.TrangThai == "Hoàn thành" || ct.YeuCauThuGom.TrangThai == "Thu gom thành công") &&
                                 ct.YeuCauThuGom.ThoiGianHoanThanh >= startDate && ct.YeuCauThuGom.ThoiGianHoanThanh < endDateForQuery)
                    .GroupBy(ct => new { ct.M_LoaiSP, ct.M_DonViTinh })
                    .Select(g => new { g.Key.M_LoaiSP, g.Key.M_DonViTinh, TongNhap = g.Sum(ct => ct.SoLuong) })
                    .ToListAsync();

                var exportsGrouped = await _context.ChiTietPhieuXuats // Giả định
                    .Include(ct => ct.PhieuXuat)
                    .Where(ct => ct.PhieuXuat.NgayXuat >= startDate && ct.PhieuXuat.NgayXuat < endDateForQuery)
                    .GroupBy(ct => new { ct.M_LoaiSP, ct.M_DonViTinh })
                    .Select(g => new { g.Key.M_LoaiSP, g.Key.M_DonViTinh, TongXuat = g.Sum(ct => ct.SoLuong) })
                    .ToListAsync();

                var stockGrouped = await _context.TonKhos
                    .GroupBy(tk => new { tk.M_LoaiSP, tk.M_DonViTinh })
                    .Select(g => new { g.Key.M_LoaiSP, g.Key.M_DonViTinh, TonHienTai = g.Sum(tk => tk.SoLuong) })
                    .ToListAsync();


                // --- Tạo ViewModel List ---
                foreach (var productInfo in allRelevantProducts)
                {
                    var importData = importsGrouped.FirstOrDefault(i => i.M_LoaiSP == productInfo.M_LoaiSP && i.M_DonViTinh == productInfo.M_DonViTinh);
                    var exportData = exportsGrouped.FirstOrDefault(e => e.M_LoaiSP == productInfo.M_LoaiSP && e.M_DonViTinh == productInfo.M_DonViTinh);
                    var stockData = stockGrouped.FirstOrDefault(s => s.M_LoaiSP == productInfo.M_LoaiSP && s.M_DonViTinh == productInfo.M_DonViTinh);

                    viewModel.ThongKeItems.Add(new ThongKeItemViewModel
                    {
                        MaSanPham = productInfo.M_LoaiSP,
                        TenSanPham = productDetails.TryGetValue(productInfo.M_LoaiSP, out var tenSP) ? tenSP ?? productInfo.M_LoaiSP : productInfo.M_LoaiSP,
                        MaDonViTinh = productInfo.M_DonViTinh,
                        TenDonViTinh = unitDetails.TryGetValue(productInfo.M_DonViTinh, out var tenDVT) ? tenDVT ?? productInfo.M_DonViTinh : productInfo.M_DonViTinh,
                        TongNhap = importData?.TongNhap ?? 0,
                        TongXuat = exportData?.TongXuat ?? 0,
                        TonHienTai = stockData?.TonHienTai ?? 0
                    });
                }

                // Sắp xếp kết quả (ví dụ theo tên sản phẩm)
                viewModel.ThongKeItems = viewModel.ThongKeItems.OrderBy(item => item.TenSanPham).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu thống kê Nhập Xuất.");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải dữ liệu thống kê.";
                // ViewModel vẫn được trả về với ngày tháng đã set
            }

            return View(viewModel);
        }
    }
}