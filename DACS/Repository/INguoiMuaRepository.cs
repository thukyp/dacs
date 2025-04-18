// DACS/Repositories/INguoiMuaRepository.cs
using DACS.Models; // Namespace chứa model NguoiMua
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DACS.Repositories
{
    public interface INguoiMuaRepository
    {
        // Lấy tất cả người mua (có thể thêm phân trang/sắp xếp sau)
        Task<IEnumerable<NguoiMua>> GetAllAsync();

        // Lấy người mua theo Mã Người Mua (Khóa chính)
        Task<NguoiMua?> GetByMaAsync(string maNguoiMua);

        // Lấy người mua theo UserId (Khóa ngoại từ ApplicationUser) - Rất quan trọng cho Profile
        Task<NguoiMua?> GetByUserIdAsync(string userId);

        // Thêm người mua mới
        Task AddAsync(NguoiMua nguoiMua);

        // Cập nhật thông tin người mua
        void Update(NguoiMua nguoiMua); // Update thường không cần async

        // Xóa người mua
        void Delete(NguoiMua nguoiMua); // Delete thường không cần async

        // Lưu các thay đổi vào database (Quan trọng!)
        Task SaveChangesAsync();
    }
}