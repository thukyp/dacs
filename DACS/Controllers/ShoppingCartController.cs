using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using DACS.Models;
using DACS.Repositories;
using YourNameSpace.Extensions;
using DACS.Extention;
using Microsoft.AspNetCore.Http;


namespace DACS.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, ISanPhamRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Checkout()
        {

            return View(new DonHang());
        }
       
        [HttpPost]
        public async Task<IActionResult> Checkout(DonHang order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                // Có thể cần truyền lại giỏ hàng vào View nếu muốn hiển thị lại
                ViewBag.Cart = cart;
                return View(order);
            }

            var user = await _userManager.GetUserAsync(User);

            order.M_DonHang = Guid.NewGuid().ToString().Substring(0, 10);
            order.M_KhachHang = user.Id;
            order.NgayDat = DateTime.UtcNow;
            order.NgayGiao = null;
            order.TrangThai ??= "Chờ xử lý";
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Khoiluong);

            // Gán phương thức thanh toán nếu không có trong model binding
            if (string.IsNullOrEmpty(order.M_PhuongThuc))
            {
                order.M_PhuongThuc = Request.Form["M_PhuongThuc"];
            }

            order.ChiTietDatHangs = cart.Items.Select(i => new ChiTietDatHang
            {
                M_CTDatHang = Guid.NewGuid().ToString(),
                M_DonHang = order.M_DonHang,
                M_SanPham = i.ProductId,
                Khoiluong = i.Khoiluong,
                GiaDatHang = i.Price,
                Quantity = i.Quantity,
                M_KhachHang = user.Id
            }).ToList();

            _context.DonHangs.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            return View("OrderCompleted", order.M_DonHang);
        }




        public async Task<IActionResult> AddToCart(string productId, float khoiluong, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = await GetProductFromDatabase(productId);
            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.TenSanPham,
                Price = product.Gia,
                Quantity = quantity,
                Khoiluong = khoiluong
            };
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ??
            new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("index");
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ??
new ShoppingCart();
            return View(cart);
        }
        // Các actions khác...
        private async Task<SanPham> GetProductFromDatabase(string productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }
        

        public IActionResult RemoveFromCart(string productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        
       

    }
}
