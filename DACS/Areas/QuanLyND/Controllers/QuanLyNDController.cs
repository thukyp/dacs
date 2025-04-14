using Đồ_án_cs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Đồ_án_cs.Areas.QuanLyND.Controllers
{
    [Area("QuanLyND")]
    [Authorize(Roles ="QuanLyND, Owner")]
    public class QuanLyNDController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult QuanLyPhanHoi()
        {
            return View();
        }
        public IActionResult QuanLyHanhDong()
        {
            return View();
        }
        public IActionResult QuanLyHoTro()
        {
            return View();
        }
        public IActionResult QuanLyBaoCao()
        {
            return View();
        }
    }
}
