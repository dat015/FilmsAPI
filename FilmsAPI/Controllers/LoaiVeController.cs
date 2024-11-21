using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiVeController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public LoaiVeController()
        {
            _db = new FilmsDbContext();
        }

        // GET: api/LoaiVe
        [HttpGet]
        public async Task<IActionResult> GetAllLoaiVe()
        {
            try
            {
                var loaiVes = await _db.LoaiVes.ToListAsync();
                return Ok(loaiVes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi lấy dữ liệu: " + ex.Message });
            }
        }

        // GET: api/LoaiVe/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoaiVeById(int id)
        {
            try
            {
                var loaiVe = await _db.LoaiVes.FindAsync(id);

                if (loaiVe == null)
                {
                    return NotFound(new { message = "Không tìm thấy Loại vé với ID " + id });
                }

                return Ok(loaiVe);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi lấy dữ liệu: " + ex.Message });
            }
        }

        // POST: api/LoaiVe
        [HttpPost]
        public async Task<IActionResult> CreateLoaiVe([FromBody] LoaiVe loaiVe)
        {
            if (loaiVe == null || string.IsNullOrWhiteSpace(loaiVe.TenLoaiVe))
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                // Kiểm tra xem loại vé đã tồn tại chưa (nếu tên trùng nhau)
                var existingLoaiVe = await _db.LoaiVes
                    .Where(l => l.TenLoaiVe.ToLower() == loaiVe.TenLoaiVe.ToLower())
                    .FirstOrDefaultAsync();

                if (existingLoaiVe != null)
                {
                    return BadRequest(new { message = "Loại vé này đã tồn tại" });
                }

                _db.LoaiVes.Add(loaiVe);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLoaiVeById), new { id = loaiVe.MaLoai }, loaiVe);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi thêm mới loại vé: " + ex.Message });
            }
        }

        // PUT: api/LoaiVe/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoaiVe(int id, [FromBody] LoaiVe loaiVe)
        {
            if (loaiVe == null || id != loaiVe.MaLoai)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                var existingLoaiVe = await _db.LoaiVes.FindAsync(id);

                if (existingLoaiVe == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại vé cần cập nhật" });
                }

                existingLoaiVe.TenLoaiVe = loaiVe.TenLoaiVe;
                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật loại vé thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi cập nhật loại vé: " + ex.Message });
            }
        }

        // DELETE: api/LoaiVe/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiVe(int id)
        {
            try
            {
                var loaiVe = await _db.LoaiVes.FindAsync(id);

                if (loaiVe == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại vé cần xóa" });
                }

                _db.LoaiVes.Remove(loaiVe);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Xóa loại vé thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi xóa loại vé: " + ex.Message });
            }
        }
    }
}
