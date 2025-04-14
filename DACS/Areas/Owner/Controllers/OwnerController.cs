using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DoanhThu()
        {
            return View();
        }
        public IActionResult QuanLyDonHang()
        {
            return View();
        }
        public IActionResult QuanLyGiaoDichThanhToan()
        {
            return View();
        }
        public IActionResult QuanLyTaiKhoan()
        {
            return View();
        }
        public IActionResult Quanlysanpham()
        {
            return View();
        }
        public IActionResult ThuGom()
        {
            return View();
        }
    }
    
}
