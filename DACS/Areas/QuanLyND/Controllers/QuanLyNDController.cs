using DACS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLyND.Controllers
{
    [Area("QuanLyND")]
    [Authorize(Roles ="QuanLyND, Owner")]
    public class QuanLyDHController : Controller
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
