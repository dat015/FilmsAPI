using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilmsAPI.Filters;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]
    public class LoaiPhimController : ControllerBase
    {
        private readonly FilmsDbContext _context;

        public LoaiPhimController()
        {
            _context = new FilmsDbContext();
        }

        // GET: api/LoaiPhim
        [HttpGet]
        public async Task<IActionResult> GetAllLoaiPhim()
        {
            try
            {
                var loaiPhims = await _context.LoaiPhims
                                    .Include(p => p.TheLoaiCuaPhims)
                                    .ToListAsync();

                if (loaiPhims == null || loaiPhims.Count == 0)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim nào." });
                }

                return Ok(new { message = "Lấy danh sách loại phim thành công.", data = loaiPhims });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách loại phim: " + ex.Message });
            }
        }

        // GET: api/LoaiPhim/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoaiPhimById(int id)
        {
            try
            {
                var loaiPhim = await _context.LoaiPhims
                    .FirstOrDefaultAsync(lp => lp.MaTheLoai == id);

                if (loaiPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim với MaTheLoai " + id });
                }

                return Ok(new { message = "Lấy thông tin loại phim thành công.", data = loaiPhim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy thông tin loại phim: " + ex.Message });
            }
        }

        // POST: api/LoaiPhim
        [HttpPost]
        public async Task<IActionResult> CreateLoaiPhim([FromBody] LoaiPhim loaiPhim)
        {
            if (loaiPhim == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                _context.LoaiPhims.Add(loaiPhim);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Thêm loại phim thành công.", data = loaiPhim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi thêm loại phim: " + ex.Message });
            }
        }

        // PUT: api/LoaiPhim/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoaiPhim(int id, [FromBody] LoaiPhim loaiPhim)
        {
            if (loaiPhim == null || id != loaiPhim.MaTheLoai)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ hoặc MaTheLoai không khớp." });
            }

            try
            {
                var existingLoaiPhim = await _context.LoaiPhims
                    .FirstOrDefaultAsync(lp => lp.MaTheLoai == id);

                if (existingLoaiPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim với MaTheLoai " + id });
                }

                // Cập nhật thông tin loại phim
                existingLoaiPhim.TenTheLoai = loaiPhim.TenTheLoai;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật loại phim thành công.", data = existingLoaiPhim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật loại phim: " + ex.Message });
            }
        }

        // DELETE: api/LoaiPhim/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiPhim(int id)
        {
            try
            {
                var loaiPhim = await _context.LoaiPhims
                    .FirstOrDefaultAsync(lp => lp.MaTheLoai == id);

                if (loaiPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim với MaTheLoai " + id });
                }

                _context.LoaiPhims.Remove(loaiPhim);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa loại phim thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa loại phim: " + ex.Message });
            }
        }
    }
}
