using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Controllers
{
    public class DS_SPController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISanPhamRepository _sanphamRepository;

        public DS_SPController(ApplicationDbContext context, ISanPhamRepository sanphamRepository)
        {
            _context = context;
            _sanphamRepository = sanphamRepository;
        }

        public async Task<IActionResult> Index()
        {
            var danhSachSanPham = await _context.SanPhams
                .Include(sp => sp.LoaiSanPham) // Include để LoaiSanPham không bị null
                .ToListAsync();

            return View(danhSachSanPham);
        }

        public async Task<IActionResult> CT_SP(string id)
        {
            var product = await _sanphamRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        public async Task<IActionResult> PhuPhamTho()
        {
            var danhSachSanPham = await _context.SanPhams
                .Include(sp => sp.LoaiSanPham) // Include để LoaiSanPham không bị null
                .ToListAsync();

            return View(danhSachSanPham);
        }
        public async Task<IActionResult> DaQuaXuLy()
        {
            var danhSachSanPham = await _context.SanPhams
                .Include(sp => sp.LoaiSanPham) // Include để LoaiSanPham không bị null
                .ToListAsync();

            return View(danhSachSanPham);
        }
        public async Task<IActionResult> ThucAnChanNuoi()
        {
            var danhSachSanPham = await _context.SanPhams
                .Include(sp => sp.LoaiSanPham) // Include để LoaiSanPham không bị null
                .ToListAsync();

            return View(danhSachSanPham);
        }
        public async Task<IActionResult> PhanBon()
        {
            var danhSachSanPham = await _context.SanPhams
                .Include(sp => sp.LoaiSanPham) // Include để LoaiSanPham không bị null
                .ToListAsync();

            return View(danhSachSanPham);
        }

        public async Task<IActionResult> NangLuongSinhKhoi()
        {
            var danhSachSanPham = await _context.SanPhams
                .Include(sp => sp.LoaiSanPham) // Include để LoaiSanPham không bị null
                .ToListAsync();

            return View(danhSachSanPham);
        }
    }
}
