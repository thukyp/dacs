using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using DACS.Models;
using DACS.Repositories;
using YourNameSpace.Extensions;
using DACS.Extention;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace DACS.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ISanPhamRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ISanPhamRepository productRepository)
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

            var user = await _userManager.GetUserAsync(User);

            // Kiểm tra xem người dùng đã có trong bảng KhachHangs chưa
            var nguoiMuaProfile = await _context.KhachHangs
                .FirstOrDefaultAsync(kh => kh.UserId == user.Id);






            // Kiểm tra hoặc tạo M_VanDon
            string vanDonId = "VD" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var vanChuyenExist = await _context.VanChuyens
                .FirstOrDefaultAsync(vc => vc.M_VanDon == vanDonId);

            if (vanChuyenExist == null)
            {
                // Nếu không có, bạn có thể tạo mới và cung cấp giá trị cho DonViVanChuyen
                vanChuyenExist = new VanChuyen
                {
                    M_VanDon = vanDonId,
                    DonViVanChuyen = "DHL" // Cung cấp giá trị cho DonViVanChuyen
                };
                _context.VanChuyens.Add(vanChuyenExist);
                await _context.SaveChangesAsync();
            }

            // Tạo mã đơn hàng tự động
            var lastOrder = _context.DonHangs
                .OrderByDescending(o => o.M_DonHang)
                .FirstOrDefault();

            int nextNumber = 1;
            if (lastOrder != null && lastOrder.M_DonHang.StartsWith("DH"))
            {
                var numberPart = lastOrder.M_DonHang.Substring(2);
                if (int.TryParse(numberPart, out int parsedNumber))
                {
                    nextNumber = parsedNumber + 1;
                }
            }

            order.M_DonHang = "DH" + nextNumber.ToString("D6");
            order.M_VanDon = vanChuyenExist.M_VanDon; // Gán M_VanDon đã xác nhận

            // Đảm bảo gán giá trị cho TrangThai
            order.TrangThai = order.TrangThai ?? "Chưa xử lý";  // Gán giá trị mặc định nếu TrangThai là null

            order.M_KhachHang = nguoiMuaProfile.M_KhachHang;
            order.NgayDat = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Khoiluong);

            // Tiến hành thêm đơn hàng vào cơ sở dữ liệu
            _context.DonHangs.Add(order);
            await _context.SaveChangesAsync();

            order.ChiTietDatHangs = cart.Items.Select(i => new ChiTietDatHang
            {
                M_KhachHang = nguoiMuaProfile.M_KhachHang,
                M_DonHang = order.M_DonHang,
                M_SanPham = i.ProductId,
                ProductId = i.ProductId,
                Khoiluong = i.Khoiluong,
                GiaDatHang = i.Price,
                M_CTDatHang = "CT" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                TongTien = (long)(i.Price * i.Khoiluong),
                NgayTao = DateTime.UtcNow,
                Quantity = i.Quantity,
                TrangThaiDonHang = "Chưa xử lý"
            }).ToList();

            _context.ChiTietDatHangs.AddRange(order.ChiTietDatHangs); // Thêm chi tiết đơn hàng vào bảng ChiTietDatHangs
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            return View("OrderCompleted", order.M_DonHang); // Trả về màn hình "Đặt hàng thành công"
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCartItem([FromBody] CartUpdateModel model)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null) return BadRequest();

            var item = cart.Items.FirstOrDefault(i => i.ProductId == model.ProductId);
            if (item != null)
            {
                item.Khoiluong = model.Khoiluong;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Ok();
        }

        public class CartUpdateModel
        {
            public string ProductId { get; set; }
            public float Khoiluong { get; set; }
        }



        public async Task<IActionResult> AddToCart(string productId, float khoiluong, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = await GetProductFromDatabase(productId);

            var cartItem = new CartItem
            {
                ProductId = product.M_SanPham,
                Name = product.TenSanPham,
                Price = product.Gia,
                Quantity = quantity,
                Khoiluong = 1
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
