using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLyXNK.Controllers
{
    [Area("QuanLyXNK")] // Chỉ định Area
    [Authorize(Roles = "Owner,QuanLyXNK")] // Ví dụ phân quyền: Chỉ Admin hoặc người có role QuanLyXNK mới vào được

    public class TonKhoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
