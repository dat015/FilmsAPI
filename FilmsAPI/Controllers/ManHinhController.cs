using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models; 
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Filters;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

    public class ManHinhController : ControllerBase
    {
        private readonly FilmsDbContext _context; 

        public ManHinhController()
        {
            _context = new FilmsDbContext();
        }

        // Lấy tất cả màn hình
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManHinh>>> GetManHinh()
        {
            var manHinhs = await _context.ManHinhs
                .ToListAsync();

            return Ok(manHinhs);
        }

        // Lấy màn hình theo MaManHinh
        [HttpGet("{mamanhinh}")]
        public async Task<ActionResult<ManHinh>> GetManHinh(int mamanhinh)
        {
            var manHinh = await _context.ManHinhs
                .Include(mh => mh.DangPhims)  // Bao gồm DangPhim liên quan
                .Include(mh => mh.PhongChieus) // Bao gồm PhongChieu liên quan
                .FirstOrDefaultAsync(mh => mh.MaManHinh == mamanhinh);

            if (manHinh == null)
            {
                return NotFound();
            }

            return Ok(manHinh);
        }

        // Thêm màn hình mới
        [HttpPost]
        public async Task<ActionResult<ManHinh>> PostManHinh(ManHinh manHinh)
        {
            try
            {
                _context.ManHinhs.Add(manHinh);
                await _context.SaveChangesAsync();

                // Trả về thông báo thành công
                return CreatedAtAction(nameof(GetManHinh), new { mamanhinh = manHinh.MaManHinh }, new { IsSuccess = true, Message = "Màn hình đã được thêm thành công." });
            }
            catch (Exception ex)
            {
                // Trả về thông báo thất bại
                return BadRequest(new { IsSuccess = false, Message = $"Lỗi khi thêm màn hình: {ex.Message}" });
            }
        }

        // Cập nhật màn hình
        [HttpPut("{mamanhinh}")]
        public async Task<IActionResult> PutManHinh(int mamanhinh, [FromBody] ManHinh manHinh)
        {
            // Lấy đối tượng màn hình hiện tại từ cơ sở dữ liệu
            var manHinhDb = await _context.ManHinhs.FindAsync(mamanhinh);
            if (manHinhDb == null)
            {
                return NotFound(new { message = "Mã màn hình không tồn tại." });
            }

            // Cập nhật tên màn hình
            manHinhDb.TenManHinh = manHinh.TenManHinh;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // Xóa màn hình
        [HttpDelete("{mamanhinh}")]
        public async Task<IActionResult> DeleteManHinh(int mamanhinh)
        {
            var manHinh = await _context.ManHinhs.FindAsync(mamanhinh);

            if (manHinh == null)
            {
                return NotFound();
            }

            _context.ManHinhs.Remove(manHinh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Kiểm tra sự tồn tại của màn hình
        private bool ManHinhExists(int mamanhinh)
        {
            return _context.ManHinhs.Any(e => e.MaManHinh == mamanhinh);
        }
    }
}
