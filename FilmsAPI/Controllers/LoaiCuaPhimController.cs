using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiCuaPhimController : ControllerBase
    {
        private readonly FilmsDbContext _context;

        public LoaiCuaPhimController()
        {
            _context = new FilmsDbContext();
        }

        [HttpGet]
        public async Task<ActionResult> GetLoaiCuaPhims()
        {
            var loaiCuaPhims = await _context.TheLoaiCuaPhims
                .ToListAsync();

            return Ok(loaiCuaPhims);
        }

        [HttpGet("{maphim}/{matheloai}")]
        public async Task<ActionResult> GetLoaiCuaPhim(int maphim, int matheloai)
        {
            var loaiCuaPhim = await _context.TheLoaiCuaPhims
                .FirstOrDefaultAsync(lcp => lcp.Maphim == maphim && lcp.MaTheLoai == matheloai);

            if (loaiCuaPhim == null)
            {
                return NotFound();
            }

            return Ok(loaiCuaPhim);
        }

        // Thêm LoaiCuaPhim mới
        [HttpPost]
        public async Task<ActionResult<TheLoaiCuaPhim>> PostLoaiCuaPhim(TheLoaiCuaPhim loaiCuaPhim)
        {
            _context.TheLoaiCuaPhims.Add(loaiCuaPhim);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoaiCuaPhim), new { maphim = loaiCuaPhim.Maphim, matheloai = loaiCuaPhim.MaTheLoai }, loaiCuaPhim);
        }

        // Cập nhật LoaiCuaPhim
        [HttpPut("{maphim}/{matheloai}")]
        public async Task<IActionResult> PutLoaiCuaPhim(int maphim, int matheloai, TheLoaiCuaPhim loaiCuaPhim)
        {
            if (maphim != loaiCuaPhim.Maphim || matheloai != loaiCuaPhim.MaTheLoai)
            {
                return BadRequest();
            }

            _context.Entry(loaiCuaPhim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiCuaPhimExists(maphim, matheloai))
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

        // Xóa LoaiCuaPhim
        [HttpDelete("{maphim}/{matheloai}")]
        public async Task<IActionResult> DeleteLoaiCuaPhim(int maphim, int matheloai)
        {
            var loaiCuaPhim = await _context.TheLoaiCuaPhims.FindAsync(maphim, matheloai);

            if (loaiCuaPhim == null)
            {
                return NotFound();
            }

            _context.TheLoaiCuaPhims.Remove(loaiCuaPhim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Kiểm tra sự tồn tại của LoaiCuaPhim
        private bool LoaiCuaPhimExists(int maphim, int matheloai)
        {
            return _context.TheLoaiCuaPhims.Any(e => e.Maphim == maphim && e.MaTheLoai == matheloai);
        }
    }
}
