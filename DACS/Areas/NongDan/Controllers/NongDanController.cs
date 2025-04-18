using DACS.Models;
using DACS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.KhachHang.Controllers
{
    [Area("NongDan")]
    [Authorize(Roles = "NongDan")]
    public class NongDanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NongDanController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HoSoCaNhan()
        {
            return View();
        }
        public IActionResult LichSuDonHang()
        {
            return View();
        }
        public IActionResult LichSuGiaoDich()
        {
            return View();
        }

        // GET: /NongDan/Profile/Edit
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);
            // Không cần kiểm tra null userId nữa vì [Authorize] đã đảm bảo đăng nhập

            var nongDanProfile = await _context.NongDans
                                        .FirstOrDefaultAsync(nd => nd.UserId == userId); // *** Dùng UserId để tìm ***

            if (nongDanProfile == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hồ sơ Nông dân liên kết.";
                // Có thể chuyển hướng đến trang tạo hồ sơ nếu cần
                return View(); // Trả về View rỗng
            }

            // Map dữ liệu từ Entity sang ViewModel
            var viewModel = new EditNongDanProfile
            {
                Ten_NongDan = nongDanProfile.Ten_NongDan,
                Email_NongDan = nongDanProfile.Email_NongDan,
                SDT_NongDan = nongDanProfile.SDT_NongDan,
                DiaChi_NongDan = nongDanProfile.DiaChi_NongDan
                // Map các trường khác nếu có
            };

            return View(viewModel); // Chỉ trả về ViewModel của Nông Dân
        }

        // POST: /NongDan/Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditNongDanProfile model)
        {
            // Không cần kiểm tra Role nữa vì đã có [Authorize] ở Controller
            if (!ModelState.IsValid)
            {
                return View(model); // Trả về view với lỗi validation
            }

            var userId = _userManager.GetUserId(User);
            var nongDanProfile = await _context.NongDans.FirstOrDefaultAsync(nd => nd.UserId == userId);

            if (nongDanProfile == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hồ sơ để cập nhật.";
                return View(model); // Hoặc RedirectToAction(nameof(Edit));
            }

            // Cập nhật thông tin từ ViewModel vào Entity
            nongDanProfile.Ten_NongDan = model.Ten_NongDan;
            nongDanProfile.Email_NongDan = model.Email_NongDan;
            nongDanProfile.SDT_NongDan = model.SDT_NongDan;
            nongDanProfile.DiaChi_NongDan = model.DiaChi_NongDan;
            // Cập nhật các trường khác nếu có

            try
            {
                _context.Update(nongDanProfile);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật hồ sơ (Concurrency).";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi không mong muốn xảy ra.";
                // Ghi log lỗi ex chi tiết
                return View(model);
            }

            return RedirectToAction(nameof(Edit)); // Quay lại trang chỉnh sửa
        }
    }
}

