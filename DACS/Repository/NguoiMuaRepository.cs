// DACS/Repositories/NguoiMuaRepository.cs
using DACS.Models;
using Microsoft.EntityFrameworkCore; // Cần cho Entity Framework Core
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DACS.Repositories
{
    public class NguoiMuaRepository : INguoiMuaRepository
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext vào Repository
        public NguoiMuaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NguoiMua nguoiMua)
        {
            await _context.NguoiMuas.AddAsync(nguoiMua);
            // Chưa gọi SaveChangesAsync() ở đây
        }

        public void Delete(NguoiMua nguoiMua)
        {
            _context.NguoiMuas.Remove(nguoiMua);
            // Chưa gọi SaveChangesAsync() ở đây
        }

        public async Task<IEnumerable<NguoiMua>> GetAllAsync()
        {
            // Có thể thêm .Include() nếu muốn lấy kèm dữ liệu liên quan
            return await _context.NguoiMuas.ToListAsync();
        }

        public async Task<NguoiMua?> GetByMaAsync(string maNguoiMua)
        {
            return await _context.NguoiMuas
                                 // .Include(nm => nm.User) // Ví dụ Include User nếu cần
                                 .FirstOrDefaultAsync(nm => nm.M_NguoiMua == maNguoiMua);
        }

        public async Task<NguoiMua?> GetByUserIdAsync(string userId)
        {
            return await _context.NguoiMuas
                                 // .Include(nm => nm.User) // Ví dụ Include User nếu cần
                                 .FirstOrDefaultAsync(nm => nm.UserId == userId);
        }

        public void Update(NguoiMua nguoiMua)
        {
            _context.NguoiMuas.Update(nguoiMua);
            // Chưa gọi SaveChangesAsync() ở đây
        }

        // Phương thức quan trọng để lưu thay đổi
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}