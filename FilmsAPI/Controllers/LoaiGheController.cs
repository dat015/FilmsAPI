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

    public class LoaiGheController : ControllerBase
    {
        private readonly FilmsDbContext _context;

        // Constructor
        public LoaiGheController()
        {
            _context = new FilmsDbContext();
        }

        // GET: api/LoaiGhe
        [HttpGet]
        public async Task<IActionResult> GetAllLoaiGhe()
        {
            var loaiGhe = await _context.LoaiGhes
                .ToListAsync();

            return Ok(loaiGhe);
        }

        // GET: api/LoaiGhe/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoaiGheById(int id)
        {
            try
            {
                var loaiGhe = await _context.LoaiGhes
                    .FirstOrDefaultAsync(lg => lg.MaLoai == id);

                if (loaiGhe == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại ghế với MaLoai " + id });
                }

                return Ok(loaiGhe);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/LoaiGhe
        [HttpPost]
        public async Task<IActionResult> CreateLoaiGhe([FromBody] LoaiGhe loaiGhe)
        {
            if (loaiGhe == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                _context.LoaiGhes.Add(loaiGhe);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLoaiGheById), new { id = loaiGhe.MaLoai }, loaiGhe);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/LoaiGhe/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoaiGhe(int id, [FromBody] LoaiGhe loaiGhe)
        {
            if (loaiGhe == null || loaiGhe.MaLoai != id)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ hoặc MaLoai không khớp." });
            }

            try
            {
                var existingLoaiGhe = await _context.LoaiGhes
                    .FirstOrDefaultAsync(lg => lg.MaLoai == id);

                if (existingLoaiGhe == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại ghế với MaLoai " + id });
                }

                // Cập nhật thông tin loại ghế
                existingLoaiGhe.TenLoaiGhe = loaiGhe.TenLoaiGhe;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật loại ghế thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/LoaiGhe/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiGhe(int id)
        {
            try
            {
                var loaiGhe = await _context.LoaiGhes
                    .FirstOrDefaultAsync(lg => lg.MaLoai == id);

                if (loaiGhe == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại ghế với MaLoai " + id });
                }

                _context.LoaiGhes.Remove(loaiGhe);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa loại ghế thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
