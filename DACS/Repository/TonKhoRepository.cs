// File: Repositories/TonKhoRepository.cs
using DACS.Models;
using DACS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DACS.Repository
{
    public class TonKhoRepository : ITonKhoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TonKhoRepository> _logger; // Thêm Logger nếu cần

        public TonKhoRepository(ApplicationDbContext context, ILogger<TonKhoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(IEnumerable<TonKho> Items, int TotalCount)> GetPagedTonKhoAsync(
            string? searchTerm, string? maKhoFilter, int pageIndex, int pageSize, bool trackChanges = false)
        {
            var query = _context.TonKhos.AsQueryable();

            if (!trackChanges)
            {
                query = query.AsNoTracking(); // Không theo dõi thay đổi để tăng hiệu suất đọc
            }

            // Nối các bảng cần thiết để lấy tên
            query = query.Include(tk => tk.KhoHang)
                         .Include(tk => tk.LoaiSanPham)
                         .Include(tk => tk.DonViTinh);

            // --- Áp dụng Bộ lọc ---
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerSearchTerm = searchTerm.ToLower().Trim();
                query = query.Where(tk =>
                    (tk.LoaiSanPham != null && tk.LoaiSanPham.TenLoai != null && tk.LoaiSanPham.TenLoai.ToLower().Contains(lowerSearchTerm)) ||
                    (tk.LoaiSanPham != null && tk.M_LoaiSP != null && tk.M_LoaiSP.ToLower().Contains(lowerSearchTerm)) || // Tìm theo mã SP
                    (!string.IsNullOrEmpty(tk.SoLo) && tk.SoLo.ToLower().Contains(lowerSearchTerm)) // Tìm theo số lô
                );
            }

            if (!string.IsNullOrEmpty(maKhoFilter) && maKhoFilter.ToLower() != "all")
            {
                query = query.Where(tk => tk.MaKho == maKhoFilter);
            }

            // --- Lấy Tổng số bản ghi (sau khi lọc) ---
            var totalItems = await query.CountAsync();

            // --- Sắp xếp (Ví dụ: Theo Tên SP, rồi Tên Kho) ---
            query = query.OrderBy(tk => tk.LoaiSanPham != null ? tk.LoaiSanPham.TenLoai : tk.M_LoaiSP)
                         .ThenBy(tk => tk.KhoHang != null ? tk.KhoHang.TenKho : tk.MaKho);

            // --- Phân trang ---
            var pagedData = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedData, totalItems);
        }

        public async Task<IEnumerable<SelectListItem>> GetKhoHangOptionsAsync()
        {
            var options = await _context.KhoHangs
                .AsNoTracking()
                .OrderBy(kh => kh.TenKho)
                .Select(kh => new SelectListItem
                {
                    Value = kh.MaKho,
                    Text = $"{kh.TenKho} ({kh.MaKho})"
                })
                .ToListAsync();

            // Thêm tùy chọn "Tất cả kho" vào đầu danh sách
            options.Insert(0, new SelectListItem { Value = "all", Text = "--- Tất cả kho ---" });

            return options;
        }

        // Implement các phương thức khác (GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync) nếu cần
        // ...

        // Implement GetDinhMucToiThieuAsync nếu cần
        // public async Task<long?> GetDinhMucToiThieuAsync(string maLoaiSP)
        // {
        //     // Logic để lấy định mức từ DB (ví dụ từ bảng LoaiSanPham hoặc bảng cấu hình riêng)
        //     var loaiSp = await _context.LoaiSanPhams.FirstOrDefaultAsync(sp => sp.MaLoai == maLoaiSP);
        //     // Giả sử LoaiSanPham có thuộc tính DinhMucTonKhoToiThieu
        //     // return loaiSp?.DinhMucTonKhoToiThieu;
        //     return null; // Trả về null nếu không tìm thấy hoặc chưa có định mức
        // }
    }
}