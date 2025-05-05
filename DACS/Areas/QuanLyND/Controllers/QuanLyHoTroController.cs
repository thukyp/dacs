using DACS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLyND.Controllers
{
    public class QuanLyHoTroController : Controller
    {
        [Area("QuanLyND")]
        [Authorize(Roles = SD.Role_Owner + "," + SD.Role_QuanLyND)] // Thêm phân quyền
        public IActionResult Index()
        {
            return View();
        }
    }
}
