using DACS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLyDH.Controllers
{
    [Area("QuanLyDH")]
    [Authorize(Roles = "QuanLyDH, Owner")]
    public class QuanLyDHController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult QuanLyThanhToan()
        {
            return View();
        }
    }
}
