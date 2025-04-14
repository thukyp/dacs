using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.QuanLySP.Controllers
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
