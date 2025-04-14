using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Đồ_án_cs.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    [Authorize(Roles = "KhachHang")]
    public class KhachHangController : Controller
    {
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
    }
}
