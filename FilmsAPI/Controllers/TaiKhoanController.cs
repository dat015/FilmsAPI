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
            if (mataikhoan != taiKhoan.MaTaiKhoan)
            {
                return BadRequest();
            }

            _context.Entry(taiKhoan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(mataikhoan))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
