using System.Globalization;
using DACS.Models;
using DACS.Models.ViewModels;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    [Authorize(Roles = SD.Role_KhachHang)]
    public class KhachHangController : Controller
    {
        private readonly ApplicationDbContext _context; // <<< Giữ lại DbContext để truy vấn DonHang (trừ khi bạn tạo OrderRepository)
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INguoiMuaRepository _nguoiMuaRepo; // <<< Inject Repository NguoiMua
        private readonly ILogger<KhachHangController> _logger;

        public KhachHangController(
     ApplicationDbContext context, // <<< Thêm lại tham số context
     UserManager<ApplicationUser> userManager,
     IWebHostEnvironment webHostEnvironment,
     INguoiMuaRepository nguoiMuaRepository,
     ILogger<KhachHangController> logger)
        {
            _context = context; // <<< Gán lại context
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _nguoiMuaRepo = nguoiMuaRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge(); // Chưa đăng nhập
            }

            // Tạo ViewModel để chứa dữ liệu
            var viewModel = new KhachHangDashboardViewModel();

            // 1. Lấy thông tin profile Người Mua
            var nguoiMuaProfile = await _context.KhachHangs
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(nm => nm.UserId == userId);

            if (nguoiMuaProfile != null)
            {
                viewModel.TenNguoiMua = nguoiMuaProfile.Ten_KhachHang;
                viewModel.EmailNguoiMua = nguoiMuaProfile.Email_KhachHang; // Email liên hệ
                viewModel.SdtNguoiMua = nguoiMuaProfile.SDT_KhachHang;
                // Lấy địa chỉ mặc định (ví dụ lấy từ DiaChi_NguoiMua, hoặc từ bảng Sổ Địa Chỉ nếu có)
                viewModel.DiaChiMacDinh = nguoiMuaProfile.DiaChi_KhachHang;
            }
            else
            {
                // Lấy thông tin cơ bản từ ApplicationUser nếu không có profile NguoiMua
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    viewModel.TenNguoiMua = user.FullName ?? user.UserName;
                    viewModel.EmailNguoiMua = user.Email; // Email đăng nhập
                    viewModel.SdtNguoiMua = user.PhoneNumber;
                    viewModel.DiaChiMacDinh = user.Address ?? "Chưa cập nhật";
                }
            }
            var recentOrdersData = await _context.ChiTietDatHangs
                             .Where(dh => dh.KhachHang.UserId == userId)
                             .OrderByDescending(dh => dh.NgayTao) // <<< Sửa ở đây
                             .Take(3)
                             .Select(dh => new OrderSummaryViewModel
                             {
                                 OrderId = dh.M_CTDatHang,
                                 OrderDate = (DateTime)dh.NgayTao, // <<< Sửa cả ở đây
                                 Status = dh.TrangThaiDonHang,
                                 TotalAmount = dh.TongTien
                             })
                             .ToListAsync();

            viewModel.RecentOrders = recentOrdersData;


            // 3. Truyền ViewModel tới View Index.cshtml
            return View(viewModel);
        }

        // GET: /KhachHang/KhachHang/HoSoCaNhan
        public async Task<IActionResult> HoSoCaNhan()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) { return Challenge(); } // Đảm bảo người dùng đăng nhập

            var nguoiMuaProfile = await _nguoiMuaRepo.GetByUserIdAsync(userId);

            // --- Tùy chọn: Tự động tạo hồ sơ nếu chưa có ---
            if (nguoiMuaProfile == null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                // Kiểm tra user có tồn tại không (dù hiếm khi xảy ra nếu GetUserId thành công)
                if (user == null) { return NotFound("Lỗi: Không tìm thấy tài khoản người dùng."); }

                // Tạo hồ sơ NguoiMua cơ bản
                nguoiMuaProfile = new Models.KhachHang
                {
                    // Tạo khóa chính - Đảm bảo logic này giống với RegisterModel
                    M_KhachHang = Guid.NewGuid().ToString("N").Substring(0, 10),
                    UserId = userId,
                    // Lấy thông tin mặc định từ ApplicationUser nếu có
                    Ten_KhachHang = user.FullName ?? user.UserName ?? "Chưa đặt tên", // Ưu tiên FullName, rồi UserName
                    Email_KhachHang = user.Email ?? "Chưa có email", // Email này là email liên hệ, có thể khác email đăng nhập
                    SDT_KhachHang = user.PhoneNumber ?? "Chưa có SĐT",
                    DiaChi_KhachHang = user.Address ?? "Chưa có địa chỉ", // Giả sử ApplicationUser có Address

                };

                try
                {
                    await _nguoiMuaRepo.AddAsync(nguoiMuaProfile);
                    await _nguoiMuaRepo.SaveChangesAsync();
                    _logger.LogInformation($"Auto-created NguoiMua profile for User ID: {userId}"); // Cần inject ILogger nếu muốn ghi log
                    TempData["InfoMessage"] = "Hồ sơ của bạn đã được khởi tạo. Vui lòng cập nhật thông tin chi tiết.";
                    // Sau khi tạo thành công, nguoiMuaProfile đã có giá trị và code sẽ chạy tiếp xuống dưới
                }
                catch (Exception ex) // Bắt lỗi chung hoặc DbUpdateException
                {
                    _logger.LogError(ex, $"Error auto-creating NguoiMua profile for User ID: {userId}"); // Ghi log lỗi
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo hồ sơ Người mua của bạn.";
                    // Không tìm thấy hồ sơ, trả về View lỗi hoặc null
                    return View("HoSoCaNhan", null); // Hoặc return View("Error");
                }
            }
            // --- Kết thúc Tùy chọn tự động tạo ---

            // Nếu vẫn là null (do tạo lỗi hoặc không bật tùy chọn trên)
            if (nguoiMuaProfile == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hồ sơ Người mua.";
                return View("HoSoCaNhan", null);
            }


            // Map dữ liệu từ NguoiMua sang ViewModel để hiển thị
            // Giả sử bạn dùng EditNguoiMuaProfile ViewModel để hiển thị luôn
            var viewModel = new EditNguoiMuaProfile
            {
                // Gán các thuộc tính từ nguoiMuaProfile sang viewModel
                Ten_NguoiMua = nguoiMuaProfile.Ten_KhachHang,
                Email_NguoiMua = nguoiMuaProfile.Email_KhachHang,
                SDT_NguoiMua = nguoiMuaProfile.SDT_KhachHang,
                DiaChi_NguoiMua = nguoiMuaProfile.DiaChi_KhachHang,
                Gender = nguoiMuaProfile.Gender,
                AvatarUrl = nguoiMuaProfile.AvatarUrl
                // Gán thêm các trường khác nếu ViewModel của bạn có
            };

            return View("HoSoCaNhan", viewModel);
        }

        // GET: /KhachHang/KhachHang/Edit
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) { return Challenge(); }

            var nguoiMuaProfile = await _nguoiMuaRepo.GetByUserIdAsync(userId);

            // Nếu không tìm thấy, chuyển hướng về HoSoCaNhan
            // HoSoCaNhan sẽ xử lý việc hiển thị lỗi hoặc tự động tạo hồ sơ
            if (nguoiMuaProfile == null)
            {
                TempData["InfoMessage"] = "Không tìm thấy hồ sơ, đang kiểm tra..."; // Thông báo nhẹ nhàng
                return RedirectToAction(nameof(HoSoCaNhan));
            }

            // Map sang ViewModel để hiển thị form Edit
            var viewModel = new EditNguoiMuaProfile
            {
                // Gán các thuộc tính từ nguoiMuaProfile sang viewModel
                Ten_NguoiMua = nguoiMuaProfile.Ten_KhachHang,
                Email_NguoiMua = nguoiMuaProfile.Email_KhachHang,
                SDT_NguoiMua = nguoiMuaProfile.SDT_KhachHang,
                DiaChi_NguoiMua = nguoiMuaProfile.DiaChi_KhachHang,
                Gender = nguoiMuaProfile.Gender,
                AvatarUrl = nguoiMuaProfile.AvatarUrl
                // Gán M_NguoiMua nếu bạn cần nó trong form (ví dụ: hidden field)
                // viewModel.M_NguoiMua = nguoiMuaProfile.M_NguoiMua;
            };

            return View("Edit", viewModel); // Truyền ViewModel tới View Edit.cshtml
        }


        // POST: /KhachHang/KhachHang/Edit
        // Action POST Edit của bạn về cơ bản đã ổn với việc dùng Repository.
        // Giữ nguyên logic đó vì nó phù hợp với cấu trúc NguoiMua riêng biệt.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditNguoiMuaProfile model) // Giả sử ViewModel tên là EditNguoiMuaProfile
        {
            if (!ModelState.IsValid)
            {
                // Load lại AvatarUrl nếu validation fail (logic này có vẻ ổn)
                var userIdForUrl = _userManager.GetUserId(User);
                var profileForUrl = await _nguoiMuaRepo.GetByUserIdAsync(userIdForUrl);
                if (profileForUrl != null) { model.AvatarUrl = profileForUrl.AvatarUrl; }
                return View("Edit", model);
            }

            var userId = _userManager.GetUserId(User);
            var nguoiMuaProfile = await _nguoiMuaRepo.GetByUserIdAsync(userId); // Lấy entity từ Repo

            if (nguoiMuaProfile == null)
            {
                TempData["ErrorMessage"] = "Lỗi: Không tìm thấy hồ sơ để cập nhật.";
                return RedirectToAction(nameof(HoSoCaNhan)); // Hoặc trang lỗi
            }

            // --- Cập nhật entity NguoiMua ---
            bool hasChanges = false; // Biến kiểm tra xem có thay đổi thực sự không

            if (nguoiMuaProfile.Ten_KhachHang != model.Ten_NguoiMua) { nguoiMuaProfile.Ten_KhachHang = model.Ten_NguoiMua; hasChanges = true; }
            if (nguoiMuaProfile.Email_KhachHang != model.Email_NguoiMua) { nguoiMuaProfile.Email_KhachHang = model.Email_NguoiMua; hasChanges = true; }
            if (nguoiMuaProfile.SDT_KhachHang != model.SDT_NguoiMua) { nguoiMuaProfile.SDT_KhachHang = model.SDT_NguoiMua; hasChanges = true; }
            if (nguoiMuaProfile.DiaChi_KhachHang != model.DiaChi_NguoiMua) { nguoiMuaProfile.DiaChi_KhachHang = model.DiaChi_NguoiMua; hasChanges = true; }
            if (nguoiMuaProfile.Gender != model.Gender) { nguoiMuaProfile.Gender = model.Gender; hasChanges = true; }


            // --- Xử lý upload ảnh ---
            string? oldImagePath = nguoiMuaProfile.AvatarUrl; // Lưu đường dẫn cũ
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                // Thêm kiểm tra kiểu file và kích thước nếu cần
                if (model.ProfileImageFile.Length > 2 * 1024 * 1024) // Ví dụ giới hạn 2MB
                {
                    ModelState.AddModelError("ProfileImageFile", "Kích thước ảnh không được vượt quá 2MB.");
                    // Không gán lại AvatarUrl vì cần giữ ảnh cũ khi có lỗi
                    return View("Edit", model);
                }
                // Kiểm tra kiểu file MIME Type nếu cần

                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImageFile.FileName); // Thêm phần mở rộng file
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa có

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImageFile.CopyToAsync(fileStream);
                    }
                    var newAvatarUrl = "/images/avatars/" + uniqueFileName; // Đường dẫn lưu trong DB

                    if (nguoiMuaProfile.AvatarUrl != newAvatarUrl)
                    {
                        nguoiMuaProfile.AvatarUrl = newAvatarUrl;
                        hasChanges = true;

                        // Xóa ảnh cũ sau khi đã chắc chắn lưu ảnh mới thành công (hoặc sau khi SaveChanges)
                        if (!string.IsNullOrEmpty(oldImagePath))
                        {
                            string oldPhysicalPath = Path.Combine(_webHostEnvironment.WebRootPath, oldImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldPhysicalPath))
                            {
                                try { System.IO.File.Delete(oldPhysicalPath); } catch (Exception ex) { _logger.LogError(ex, $"Error deleting old avatar: {oldPhysicalPath}"); } // Ghi log nếu xóa lỗi
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading profile image."); // Ghi log
                    ModelState.AddModelError("ProfileImageFile", "Lỗi xảy ra khi tải ảnh lên.");
                    model.AvatarUrl = oldImagePath; // Giữ lại ảnh cũ khi có lỗi upload
                    return View("Edit", model);
                }
            }

            // Chỉ lưu nếu thực sự có thay đổi
            if (hasChanges)
            {
                try
                {
                    _nguoiMuaRepo.Update(nguoiMuaProfile); // Đánh dấu Update
                    await _nguoiMuaRepo.SaveChangesAsync(); // Lưu thay đổi vào DB
                    TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["ErrorMessage"] = "Lỗi: Dữ liệu đã bị thay đổi bởi người khác. Vui lòng tải lại trang và thử lại.";
                    model.AvatarUrl = oldImagePath; // Giữ ảnh cũ
                    return View("Edit", model);
                }
                catch (Exception ex) // Bắt lỗi chung hoặc DbUpdateException
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật hồ sơ.";
                    _logger.LogError(ex, "Error updating NguoiMua profile."); // Ghi log lỗi
                    model.AvatarUrl = oldImagePath; // Giữ ảnh cũ
                    return View("Edit", model);
                }
            }
            else
            {
                TempData["InfoMessage"] = "Không có thông tin nào được thay đổi.";
            }


            return RedirectToAction(nameof(HoSoCaNhan)); // Chuyển về trang hiển thị
        }
        public async Task<IActionResult> LichSuDonHang(string statusFilter, string timeFilter, int page = 1)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge(); // Chưa đăng nhập
            }

            // --- Xử lý logic lọc và truy vấn ---
            var query = _context.ChiTietDatHangs // Hoặc _orderRepo.GetOrdersQuery()
                              .Where(dh => dh.KhachHang.UserId == userId); // Lọc theo User ID

            // 1. Lọc theo trạng thái
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "Tất cả trạng thái") // Giả sử giá trị mặc định là "Tất cả trạng thái"
            {
                // Ánh xạ giá trị từ dropdown (pending, shipping, completed, cancelled) sang giá trị thực tế trong DB
                // Ví dụ:
                string dbStatus = statusFilter switch
                {
                    "pending" => "Đang xử lý", // Thay bằng giá trị thực trong DB của bạn
                    "shipping" => "Đang giao hàng", // Thay bằng giá trị thực
                    "completed" => "Đã giao", // Thay bằng giá trị thực
                    "cancelled" => "Đã hủy", // Thay bằng giá trị thực
                    _ => null
                };
                if (dbStatus != null)
                {
                    query = query.Where(dh => dh.TrangThaiDonHang == dbStatus);
                }
            }

            // 2. Lọc theo thời gian
            DateTime? startDate = null;
            switch (timeFilter)
            {
                case "3m":
                    startDate = DateTime.Now.AddMonths(-3);
                    break;
                case "6m":
                    startDate = DateTime.Now.AddMonths(-6);
                    break;
                case "1y":
                    startDate = DateTime.Now.AddYears(-1);
                    break;
                    // Thêm các trường hợp khác nếu cần
            }
            if (startDate.HasValue)
            {
                query = query.Where(dh => dh.NgayTao >= startDate.Value);
            }

            // 3. Sắp xếp (Mới nhất lên đầu)
            query = query.OrderByDescending(dh => dh.NgayTao);

            // 4. Phân trang
            int pageSize = 10; // Số lượng đơn hàng trên mỗi trang
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages)); // Đảm bảo page hợp lệ

            var ordersData = await query
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(dh => new OrderSummaryViewModel // Map sang ViewModel
                                    {
                                        OrderId = dh.M_CTDatHang, // Hoặc mã đơn hàng chính nếu có
                                        TotalAmount = dh.TongTien, // Đảm bảo kiểu dữ liệu phù hợp
                                        Status = dh.TrangThaiDonHang
                                    })
                                    .ToListAsync();

            // 5. Tạo ViewModel cho View
            var viewModel = new OrderHistoryViewModel
            {
                Orders = ordersData,
                PageIndex = page,
                TotalPages = totalPages,
                CurrentStatusFilter = statusFilter, // Giữ lại giá trị lọc để hiển thị trên form
                CurrentTimeFilter = timeFilter
            };

            return View(viewModel); // Trả về View LichSuDonHang.cshtml với ViewModel
        }
       
    }
}
