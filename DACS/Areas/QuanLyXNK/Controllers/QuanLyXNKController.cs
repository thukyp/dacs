using DACS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLyXNK.Controllers
{
    [Area("QuanLyXNK")]
    [Authorize(Roles = "QuanLyXNK, Owner")]
    public class QuanLyXNKController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult QuanLyDSKH()
        {
            return View();
        }
        public IActionResult QuanLyDSPX()
        {
            return View();
        }
        public IActionResult TonKho()
        {
            return View();
        }
        public IActionResult ThongKe()
        {
            return View();
        }
    }
}
