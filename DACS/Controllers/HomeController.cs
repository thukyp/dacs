using System.Diagnostics;

using System.Security.Claims;
using DACS.Areas.KhachHang.Controllers;
using DACS.Models;

using DACS.Models.ViewModels;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;



namespace DACS.Controllers

{

    public class HomeController : Controller

    {

        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context; // Thay ApplicationDbContext bằng tên DbContext của bạn

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<ApplicationUser> _userManager; // Sử dụng ApplicationUser hoặc lớp User Identity của bạn



        public HomeController(

      ILogger<HomeController> logger,

      ApplicationDbContext dbContext,

      IWebHostEnvironment webHostEnvironment,

      UserManager<ApplicationUser> userManager)

        {

            _logger = logger;

            _context = dbContext;

            _webHostEnvironment = webHostEnvironment;

            _userManager = userManager;

        }



        public IActionResult Index()

        {

            return View();

        }

        public IActionResult Introduction()

        {

            return View();

        }

        // GET: /Home/ThuGom

        [HttpGet]
        public async Task<IActionResult> ThuGomAsync() // Không cần tham số nữa
        {
            _logger.LogInformation($"GET ThuGomAsync - Initial Load");
            var model = new ThuGomViewModel();
            await LoadDropdownDataForThuGomInitial(model); // Gọi hàm helper đơn giản
            return View("ThuGom", model);
        }

        // Hàm helper đơn giản chỉ tải những gì cần ban đầu
        private async Task LoadDropdownDataForThuGomInitial(ThuGomViewModel model)
        {
            if (model == null || _context == null) return; // Kiểm tra null

            try
            {
                var loaiSpList = await _context.LoaiSanPhams.OrderBy(lsp => lsp.TenLoai).ToListAsync();
                model.LoaiSanPhamOptions = new SelectList(loaiSpList, "M_LoaiSP", "TenLoai");

                var donViTinhList = await _context.DonViTinhs.OrderBy(dvt => dvt.TenLoaiTinh).ToListAsync();
                model.DonViTinhOptions = new SelectList(donViTinhList, "M_DonViTinh", "TenLoaiTinh");

                var provinces = await _context.TinhThanhPhos.OrderBy(p => p.TenTinh).ToListAsync();
                model.ProvinceOptions = new SelectList(provinces, "MaTinh", "TenTinh");

                // Khởi tạo rỗng cho các dropdown phụ thuộc
                model.DistrictOptions = Enumerable.Empty<SelectListItem>();
                model.WardOptions = Enumerable.Empty<SelectListItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading initial dropdown data for ThuGom");
                // Gán rỗng khi lỗi
                model.LoaiSanPhamOptions ??= Enumerable.Empty<SelectListItem>();
                model.DonViTinhOptions ??= Enumerable.Empty<SelectListItem>();
                model.ProvinceOptions ??= Enumerable.Empty<SelectListItem>();
                model.DistrictOptions ??= Enumerable.Empty<SelectListItem>();
                model.WardOptions ??= Enumerable.Empty<SelectListItem>();
            }
        }


        // POST: /Home/ThuGom

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThuGom(ThuGomViewModel model)
        {
            // Chỉ kiểm tra ModelState một lần duy nhất ở đầu
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dữ liệu gửi lên không hợp lệ (ModelState invalid).");
                await LoadDropdownDataForThuGomInitial(model); // Load lại dropdown tĩnh
                return View(model);
            }

            // Lấy thông tin User/KhachHang
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Không thể xác định người dùng. Vui lòng đăng nhập lại.");
                await LoadDropdownDataForThuGomInitial(model);
                return View(model);
            }
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(kh => kh.UserId == userId);
            if (khachHang == null)
            {
                _logger.LogError($"Không tìm thấy KhachHang cho UserId: {userId}");
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng liên kết. Vui lòng cập nhật hồ sơ.";
                return RedirectToAction("Index", "Home");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            YeuCauThuGom yeuCau = null; // Khai báo ngoài để dùng trong catch logging
            try
            {
                // 1. Tạo YeuCauThuGom để có M_YeuCau
                yeuCau = new YeuCauThuGom
                {
                    M_YeuCau = GenerateRequestCode(), // Generate ID
                    M_KhachHang = khachHang.M_KhachHang,
                    NgayYeuCau = DateTime.UtcNow,
                    ThoiGianSanSang = model.PickupReadyTime.Value,
                    GhiChu = model.SupplierNotes,
                    TrangThai = "Chờ xử lý"
                };

                // 2. Xử lý ảnh (nếu có)
                List<string> savedImagePaths = new List<string>();
                if (model.ByproductImages != null && model.ByproductImages.Count > 0)
                {
                    // *** ĐỊNH NGHĨA ĐƯỜNG DẪN THƯ MỤC SAU KHI CÓ M_YeuCau ***
                    string requestUploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "thugom", yeuCau.M_YeuCau);
                    Directory.CreateDirectory(requestUploadFolder); // Tạo thư mục ngay đây

                    // Không cần try-catch lồng nếu khối catch bên ngoài đã đủ tốt
                    foreach (var file in model.ByproductImages)
                    {
                        if (file.Length > 0 && file.ContentType.StartsWith("image/"))
                        {
                            try // Vẫn nên có try nhỏ cho từng file để log lỗi file cụ thể nếu muốn
                            {
                                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                                string filePath = Path.Combine(requestUploadFolder, uniqueFileName); // Sử dụng requestUploadFolder đã định nghĩa

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }
                                string relativePath = Path.Combine("/uploads/thugom", yeuCau.M_YeuCau, uniqueFileName).Replace("\\", "/");
                                savedImagePaths.Add(relativePath);
                            }
                            catch (Exception imgEx)
                            {
                                _logger.LogError(imgEx, $"Lỗi khi xử lý file ảnh '{file.FileName}' cho yêu cầu {yeuCau.M_YeuCau}.");
                                // Có thể thêm lỗi vào ModelState nhưng không return ngay để các ảnh khác vẫn được xử lý (tùy logic mong muốn)
                                ModelState.AddModelError("ByproductImages", $"Lỗi khi xử lý file '{file.FileName}'.");
                                // Hoặc nếu muốn dừng ngay khi lỗi 1 ảnh: throw; // Để khối catch bên ngoài xử lý
                            }
                        }
                    }
                    // Nếu có lỗi ảnh được thêm vào ModelState ở trên, kiểm tra lại ở đây
                    if (!ModelState.IsValid)
                    {
                        await transaction.RollbackAsync();
                        await LoadDropdownDataForThuGomInitial(model);
                        return View(model);
                    }
                } // Kết thúc xử lý ảnh

                // 3. Tạo ChiTietThuGom
                var chiTiet = new ChiTietThuGom
                {
                    M_ChiTiet = Guid.NewGuid().ToString("N").Substring(0, 10), // <<< Tự tạo ID duy nhất
                    M_LoaiSP = model.ByproductType,
                    M_DonViTinh = model.ByproductUnit,
                    M_KhachHang = yeuCau.M_KhachHang, // <<< ASSIGN CUSTOMER ID HERE <<<
                    SoLuong = (int)model.ByproductQuantity.Value,
                    MoTa = model.ByproductDescription,
                    GiaTriMongMuon = model.ByproductValue,
                    DacTinh_CongKenh = model.CharBulky,
                    DacTinh_AmUot = model.CharWet,
                    DacTinh_Kho = model.CharDry,
                    DacTinh_DoAmCao = model.CharMoisture,
                    DacTinh_TapChat = model.CharImpure,
                    DacTinh_DaXuLy = model.CharProcessed,
                    DanhSachHinhAnh = savedImagePaths.Any() ? string.Join(";", savedImagePaths) : null,
                    MaTinh = model.SupplierProvince, // <<< Assign Province
                    MaQuan = model.SupplierDistrict, // <<< Assign District
                    MaXa = model.SupplierWard,       // <<< Assign Ward
                    DiaChi_DuongApThon = model.SupplierStreet, // Assign if this column also exists

                };
                yeuCau.ChiTietThuGoms = new List<ChiTietThuGom> { chiTiet }; // Khởi tạo List và thêm

                // 4. Lưu vào Database
                _context.YeuCauThuGoms.Add(yeuCau);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation($"Đã tạo yêu cầu thu gom {yeuCau.M_YeuCau} bởi khách hàng {khachHang.M_KhachHang}");
                TempData["SuccessMessage"] = $"Yêu cầu thu gom ({yeuCau.M_YeuCau}) của bạn đã được gửi thành công!";
                return RedirectToAction(nameof(KhachHangController.LichSuYeuCauThuGom), "KhachHang", new { area = "KhachHang" });

            }
            catch (DbUpdateException dbEx) // Bắt lỗi cụ thể từ DB
            {
                await transaction.RollbackAsync();
                _logger.LogError(dbEx, $"Lỗi DbUpdateException khi lưu YC {yeuCau?.M_YeuCau}"); // Dùng yeuCau?. để an toàn
                ModelState.AddModelError("", "Lỗi Database khi lưu: " + dbEx.InnerException?.Message);
                await LoadDropdownDataForThuGomInitial(model);
                return View(model);
            }
            catch (Exception ex) // Bắt các lỗi chung khác
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"Lỗi không xác định khi xử lý YC {yeuCau?.M_YeuCau}"); // Dùng yeuCau?. để an toàn
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi tạo yêu cầu.");
                await LoadDropdownDataForThuGomInitial(model);
                return View(model);
            }
        }

        // Hàm helper để tạo mã yêu cầu (ví dụ đơn giản)

        private string GenerateRequestCode()

        {

            // Ví dụ: YC + NămThángNgày + 4 chữ số ngẫu nhiên

            // Cần đảm bảo mã là duy nhất trong bảng YeuCauThuGom

            // Cách tốt hơn là dùng sequence trong DB hoặc GUID

            string prefix = "YC";

            string timestamp = DateTime.UtcNow.ToString("yyMMddHHmm"); // Dùng UTC

            string randomPart = new Random().Next(1000, 9999).ToString();

            // Cần kiểm tra tính duy nhất trong DB trước khi trả về mã này trong môi trường thực tế

            return $"{prefix}{timestamp}{randomPart}".Substring(0, 10); // Giới hạn 10 ký tự nếu cần

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()

        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

        public IActionResult Contact()

        {

            return View();

        }

        public IActionResult News()

        {

            return View();

        }



        // Trong HomeController.cs



        // Trong HomeController.cs



        // Trong HomeController.cs



        private async Task LoadDropdownDataForThuGom(ThuGomViewModel model)

        {

            // Ghi log để kiểm tra đầu vào

            _logger.LogInformation("--- Entering LoadDropdownDataForThuGom ---");

            _logger.LogInformation($"ViewModel received: SupplierProvince='{model?.SupplierProvince}', SupplierDistrict='{model?.SupplierDistrict}', SupplierWard='{model?.SupplierWard}'");



            if (model == null || _context == null)

            {

                _logger.LogError("Model or DbContext is NULL in LoadDropdownDataForThuGom.");

                // Gán rỗng để tránh lỗi View

                if (model != null)

                {

                    model.ProvinceOptions = Enumerable.Empty<SelectListItem>();

                    //... gán rỗng cho các options khác

                }

                return;

            }



            try

            {

                // 1. Load Loại Sản Phẩm (Kiểm tra lại tên thuộc tính!)

                var loaiSpList = await _context.LoaiSanPhams.OrderBy(lsp => lsp.TenLoai).ToListAsync();

                model.LoaiSanPhamOptions = new SelectList(loaiSpList, "M_LoaiSP", "TenLoai", model.ByproductType);

                _logger.LogInformation($"Loaded {loaiSpList.Count} LoaiSanPham. Assigned {model.LoaiSanPhamOptions?.Count() ?? -1} options.");



                // 2. Load Đơn vị tính (Kiểm tra lại tên thuộc tính!)

                var donViTinhList = await _context.DonViTinhs.OrderBy(dvt => dvt.TenLoaiTinh).ToListAsync(); // Dùng TenLoaiTinh như Model bạn cung cấp

                model.DonViTinhOptions = new SelectList(donViTinhList, "M_DonViTinh", "TenLoaiTinh", model.ByproductUnit); // Dùng TenLoaiTinh

                _logger.LogInformation($"Loaded {donViTinhList.Count} DonViTinh. Assigned {model.DonViTinhOptions?.Count() ?? -1} options.");



                // 3. Load Tỉnh/Thành phố (Đã chạy OK)

                var provinces = await _context.TinhThanhPhos.OrderBy(p => p.TenTinh).ToListAsync();

                model.ProvinceOptions = new SelectList(provinces, "MaTinh", "TenTinh", model.SupplierProvince);

                _logger.LogInformation($"Loaded {provinces.Count} Provinces. Assigned {model.ProvinceOptions?.Count() ?? -1} options.");



                // --- BẮT ĐẦU LOAD LẠI QUẬN/HUYỆN ---

                var districts = Enumerable.Empty<QuanHuyen>();

                if (!string.IsNullOrEmpty(model.SupplierProvince)) // Chỉ load khi có Tỉnh

                {

                    _logger.LogInformation($"Loading Districts for Province: {model.SupplierProvince}");

                    districts = await _context.QuanHuyens

                             .Where(q => q.MaTinh == model.SupplierProvince) // Kiểm tra tên cột MaTinh trong QuanHuyen

                                                 .OrderBy(q => q.TenQuan).ToListAsync();

                }

                // Kiểm tra lại tên thuộc tính "MaQuan", "TenQuan" trong QuanHuyen.cs

                model.DistrictOptions = new SelectList(districts, "MaQuan", "TenQuan", model.SupplierDistrict);

                _logger.LogInformation($"Loaded {districts.Count()} Districts. Assigned {model.DistrictOptions?.Count() ?? -1} options. Selected: {model.SupplierDistrict}");

                // --- KẾT THÚC LOAD QUẬN/HUYỆN ---



                // --- BẮT ĐẦU LOAD LẠI XÃ/PHƯỜNG ---

                var wards = Enumerable.Empty<XaPhuong>();

                if (!string.IsNullOrEmpty(model.SupplierDistrict)) // Chỉ load khi có Huyện

                {

                    _logger.LogInformation($"Loading Wards for District: {model.SupplierDistrict}");

                    wards = await _context.XaPhuongs

                            .Where(x => x.MaQuan == model.SupplierDistrict) // Kiểm tra tên cột MaQuan trong XaPhuong

                                               .OrderBy(x => x.TenXa).ToListAsync();

                }

                // Kiểm tra lại tên thuộc tính "MaXa", "TenXa" trong XaPhuong.cs

                model.WardOptions = new SelectList(wards, "MaXa", "TenXa", model.SupplierWard);

                _logger.LogInformation($"Loaded {wards.Count()} Wards. Assigned {model.WardOptions?.Count() ?? -1} options. Selected: {model.SupplierWard}");

                // --- KẾT THÚC LOAD XÃ/PHƯỜNG ---

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "Error loading dropdown data for ThuGom");

                // Gán danh sách rỗng khi có lỗi

                model.ProvinceOptions ??= Enumerable.Empty<SelectListItem>();

                model.DistrictOptions ??= Enumerable.Empty<SelectListItem>();

                model.WardOptions ??= Enumerable.Empty<SelectListItem>();

                model.LoaiSanPhamOptions ??= Enumerable.Empty<SelectListItem>();

                model.DonViTinhOptions ??= Enumerable.Empty<SelectListItem>();

            }

            _logger.LogInformation("--- Exiting LoadDropdownDataForThuGom ---");

        }
        // Bên trong class HomeController

        [HttpGet] // Action này chỉ đáp ứng yêu cầu GET
        public async Task<JsonResult> GetDistrictsForHome(string provinceId) // Nhận MaTinh từ AJAX
        {
            // Kiểm tra xem provinceId có được gửi lên không
            if (string.IsNullOrEmpty(provinceId))
            {
                // Trả về một danh sách rỗng nếu không có provinceId
                return Json(new List<object>());
            }

            try
            {
                // Truy vấn database để lấy các Quận/Huyện thuộc Tỉnh/TP đã chọn
                var districts = await _context.QuanHuyens // Sử dụng DbContext của bạn
                                        .Where(q => q.MaTinh == provinceId) // Lọc theo MaTinh
                                        .OrderBy(q => q.TenQuan) // Sắp xếp theo tên
                                                                 // Chọn chỉ Mã và Tên để gửi về trình duyệt (nhẹ hơn)
                                                                 // Đặt tên thuộc tính là 'id' và 'name' để JavaScript dễ dùng
                                        .Select(q => new { id = q.MaQuan, name = q.TenQuan })
                                        .ToListAsync();

                // Trả về danh sách dưới dạng JSON
                return Json(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy Quận/Huyện cho Tỉnh ID: {provinceId}");
                // Cân nhắc trả về một đối tượng lỗi để JS biết và xử lý
                // return Json(new { error = "Lỗi tải dữ liệu Quận/Huyện." });
                return Json(new List<object>()); // Hoặc trả về rỗng khi lỗi
            }
        }

        [HttpGet] // Action này chỉ đáp ứng yêu cầu GET
        public async Task<JsonResult> GetWardsForHome(string districtId) // Nhận MaQuan từ AJAX
        {
            // Kiểm tra xem districtId có được gửi lên không
            if (string.IsNullOrEmpty(districtId))
            {
                // Trả về một danh sách rỗng nếu không có districtId
                return Json(new List<object>());
            }

            try
            {
                // Truy vấn database để lấy các Xã/Phường thuộc Quận/Huyện đã chọn
                var wards = await _context.XaPhuongs // Sử dụng DbContext của bạn
                                       .Where(w => w.MaQuan == districtId) // Lọc theo MaQuan
                                       .OrderBy(w => w.TenXa) // Sắp xếp theo tên
                                                              // Chọn chỉ Mã và Tên
                                                              // Đặt tên thuộc tính là 'id' và 'name'
                                       .Select(w => new { id = w.MaXa, name = w.TenXa })
                                       .ToListAsync();

                // Trả về danh sách dưới dạng JSON
                return Json(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy Xã/Phường cho Quận ID: {districtId}");
                // return Json(new { error = "Lỗi tải dữ liệu Xã/Phường." });
                return Json(new List<object>()); // Trả về rỗng khi lỗi
            }
        }

    }

}