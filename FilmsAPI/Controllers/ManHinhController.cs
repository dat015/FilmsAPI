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
            try
            {
                var manHinhs = await _context.ManHinhs.ToListAsync();
                if (manHinhs.Count == 0)
                {
                    return Ok(new { IsSuccess = true, Message = "Không có màn hình nào trong cơ sở dữ liệu." });
                }
                return Ok(manHinhs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = $"Lỗi khi lấy danh sách màn hình: {ex.Message}" });
            }
        }

        // Lấy màn hình theo MaManHinh
        [HttpGet("{mamanhinh}")]
        public async Task<ActionResult<ManHinh>> GetManHinh(int mamanhinh)
        {
            try
            {
                var manHinh = await _context.ManHinhs
                    .Include(mh => mh.DangPhims)
                    .Include(mh => mh.PhongChieus)
                    .FirstOrDefaultAsync(mh => mh.MaManHinh == mamanhinh);

                if (manHinh == null)
                {
                    return NotFound(new { IsSuccess = false, Message = "Không tìm thấy màn hình với mã đã cho." });
                }

                return Ok(manHinh);
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = $"Lỗi khi lấy màn hình: {ex.Message}" });
            }
        }

        // Thêm màn hình mới
        [HttpPost]
        public async Task<ActionResult<ManHinh>> PostManHinh(ManHinh manHinh)
        {
            if (manHinh == null)
            {
                return BadRequest(new { IsSuccess = false, Message = "Dữ liệu màn hình không hợp lệ." });
            }

            try
            {
                _context.ManHinhs.Add(manHinh);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetManHinh), new { mamanhinh = manHinh.MaManHinh }, new { IsSuccess = true, Message = "Màn hình đã được thêm thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = $"Lỗi khi thêm màn hình: {ex.Message}" });
            }
        }

        // Cập nhật màn hình
        [HttpPut("{mamanhinh}")]
        public async Task<IActionResult> PutManHinh(int mamanhinh, [FromBody] ManHinh manHinh)
        {
            if (manHinh == null)
            {
                return BadRequest(new { IsSuccess = false, Message = "Dữ liệu cập nhật không hợp lệ." });
            }

            try
            {
                var manHinhDb = await _context.ManHinhs.FindAsync(mamanhinh);
                if (manHinhDb == null)
                {
                    return NotFound(new { IsSuccess = false, Message = "Không tìm thấy màn hình với mã đã cho." });
                }

                manHinhDb.TenManHinh = manHinh.TenManHinh;

                await _context.SaveChangesAsync();
                return Ok(new { IsSuccess = true, Message = "Cập nhật màn hình thành công." });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new { IsSuccess = false, Message = $"Lỗi cạnh tranh dữ liệu: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = $"Lỗi khi cập nhật màn hình: {ex.Message}" });
            }
        }

        // Xóa màn hình
        [HttpDelete("{mamanhinh}")]
        public async Task<IActionResult> DeleteManHinh(int mamanhinh)
        {
            try
            {
                var manHinh = await _context.ManHinhs.FindAsync(mamanhinh);

                if (manHinh == null)
                {
                    return NotFound(new { IsSuccess = false, Message = "Không tìm thấy màn hình để xóa." });
                }

                _context.ManHinhs.Remove(manHinh);
                await _context.SaveChangesAsync();
                return Ok(new { IsSuccess = true, Message = "Xóa màn hình thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = $"Lỗi khi xóa màn hình: {ex.Message}" });
            }
        }

        // Kiểm tra sự tồn tại của màn hình
        private bool ManHinhExists(int mamanhinh)
        {
            return _context.ManHinhs.Any(e => e.MaManHinh == mamanhinh);
        }
    }
}
