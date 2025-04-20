using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Controllers
{
    public class DS_SPController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DS_SPController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() // Hoặc tên Action bạn muốn
        {
            return View();
        }

        public IActionResult CT_SP() { 
            return View();
        }
    }
}
