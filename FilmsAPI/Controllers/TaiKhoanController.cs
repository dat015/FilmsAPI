using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models; 
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly FilmsDbContext _context; 

        public TaiKhoanController()
        {
            _context = new FilmsDbContext();
        }

        // Lấy tất cả tài khoản
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoan>>> GetTaiKhoans()
        {
            var taiKhoans = await _context.TaiKhoans
                .Include(tk => tk.MaKhNavigation) // Bao gồm dữ liệu từ bảng KhachHang nếu cần
                .ToListAsync();

            return Ok(taiKhoans);
        }

        // Lấy tài khoản theo MaTaiKhoan
        [HttpGet("{mataikhoan}")]
        public async Task<ActionResult<TaiKhoan>> GetTaiKhoan(int mataikhoan)
        {
            var taiKhoan = await _context.TaiKhoans
                .Include(tk => tk.MaKhNavigation) // Bao gồm dữ liệu từ bảng KhachHang nếu cần
                .FirstOrDefaultAsync(tk => tk.MaTaiKhoan == mataikhoan);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            return Ok(taiKhoan);
        }

        // Thêm tài khoản mới
        [HttpPost]
        public async Task<ActionResult<TaiKhoan>> PostTaiKhoan(TaiKhoan taiKhoan)
        {
            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaiKhoan), new { mataikhoan = taiKhoan.MaTaiKhoan }, taiKhoan);
        }

        // Cập nhật tài khoản
        [HttpPut("{mataikhoan}")]
        public async Task<IActionResult> PutTaiKhoan(int mataikhoan, TaiKhoan taiKhoan)
        {
            // Kiểm tra nếu ID không khớp
            if (mataikhoan != taiKhoan.MaTaiKhoan)
            {
                return BadRequest("Mã tài khoản không khớp.");
            }

            // Đánh dấu thực thể đã được sửa đổi
            _context.Entry(taiKhoan).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra xem tài khoản có tồn tại không
                if (!TaiKhoanExists(mataikhoan))
                {
                    return NotFound($"Không tìm thấy tài khoản với ID: {mataikhoan}");
                }

                // Nếu là lỗi khác, trả về thông báo lỗi
                return StatusCode(500, "Đã xảy ra lỗi trong khi cập nhật tài khoản.");
            }
            catch (Exception ex)
            {
                // Bắt lỗi chung và ghi log (nếu cần)
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, "Đã xảy ra lỗi không xác định.");
            }

            // Trả về phản hồi thành công rõ ràng
            return Ok("Cập nhật tài khoản thành công.");
        }

        // Xóa tài khoản
        [HttpDelete("{mataikhoan}")]
        public async Task<IActionResult> DeleteTaiKhoan(int mataikhoan)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(mataikhoan);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            _context.TaiKhoans.Remove(taiKhoan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Kiểm tra sự tồn tại của tài khoản
        private bool TaiKhoanExists(int mataikhoan)
        {
            return _context.TaiKhoans.Any(e => e.MaTaiKhoan == mataikhoan);
        }
    }
}
