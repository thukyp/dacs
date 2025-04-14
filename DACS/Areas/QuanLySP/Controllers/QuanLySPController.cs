using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Đồ_án_cs.Areas.QuanLySP.Controllers
{
    [Area("QuanLySP")]
    [Authorize(Roles = "QuanLySP, Owner")]
    public class QuanLySPController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
