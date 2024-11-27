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

        [HttpGet("{maphim}")]
        public async Task<ActionResult> GetLoaiCuaPhim(int maphim)
        {
            try
            {
                var loaiCuaPhim = await _context.TheLoaiCuaPhims
                                         .Where(l => l.Maphim == maphim)
                                         .ToListAsync();

                if (!loaiCuaPhim.Any() || loaiCuaPhim == null)
                {
                    return NotFound(new { Message = "Không tìm thấy thể loại nào cho mã phim này." });
                }
                return Ok(loaiCuaPhim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi trong quá trình xử lý.", Details = ex.Message });
            }
        }

        [HttpGet("{maphim}/{matheloai}")]
        public async Task<ActionResult> GetLoaiCuaPhim(int maphim, int matheloai)
        {
            var loaiCuaPhim = await _context.TheLoaiCuaPhims
                .FirstOrDefaultAsync(lcp => lcp.Maphim == maphim && lcp.MaTheLoai == matheloai);

            if (loaiCuaPhim == null)
            {
                return NotFound("Không tìm thấy loại của phim" );
            }

            return Ok(loaiCuaPhim);
        }
        
        [HttpPost]
        public async Task<ActionResult> PostLoaiCuaPhim(List<TheLoaiCuaPhim> loaiCuaPhim)
        {
            if (loaiCuaPhim == null || !loaiCuaPhim.Any())
            {
                return BadRequest(new { Message = "Dữ liệu loại phim không hợp lệ." });
            }

            try
            {
                _context.TheLoaiCuaPhims.AddRange(loaiCuaPhim);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Thêm loại phim thành công.", Data = loaiCuaPhim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi thêm loại phim: " + ex.Message });
            }
        }


        // Cập nhật LoaiCuaPhim
        [HttpPut("{maphim}/{matheloai}")]
        public async Task<IActionResult> PutLoaiCuaPhim(int maphim, int matheloai, TheLoaiCuaPhim loaiCuaPhim)
        {
            if (maphim != loaiCuaPhim.Maphim || matheloai != loaiCuaPhim.MaTheLoai)
            {
                return BadRequest();
            }

            _context.Entry(loaiCuaPhim).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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

            return Ok(new { Message = "Cập nhật thành công" });
        }

        // Xóa LoaiCuaPhim
        [HttpDelete("{maphim}/{matheloai}")]
        public async Task<IActionResult> DeleteLoaiCuaPhim(int maphim, int matheloai)
        {
            var loaiCuaPhim = await _context.TheLoaiCuaPhims.FindAsync(maphim, matheloai);

            if (loaiCuaPhim == null)
            {
                return BadRequest(new { Message = "Không tìm thấy loại của phim" });
            }

            _context.TheLoaiCuaPhims.Remove(loaiCuaPhim);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xóa thánh công" });
        }

        [HttpDelete(Name = "XoaLoaiPhim")]
        public async Task<ActionResult> DeleteTheLoai([FromBody] List<TheLoaiCuaPhim> dto)
        {
            try
            {
                var ids = dto.Select(t => t.Id).ToList();

                // Tìm các thể loại trong cơ sở dữ liệu
                var theLoaisToDelete = await _context.TheLoaiCuaPhims.Where(t => ids.Contains(t.Id)).ToListAsync();

                if (!theLoaisToDelete.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy thể loại nào để xóa." });
                }

                _context.TheLoaiCuaPhims.RemoveRange(theLoaisToDelete);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã xóa thành công các thể loại." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");

            }
        }



        //Kiểm tra sự tồn tại của LoaiCuaPhim
        private bool LoaiCuaPhimExists(int maphim, int matheloai)
        {
            return _context.TheLoaiCuaPhims.Any(e => e.Maphim == maphim && e.MaTheLoai == matheloai);
        }
    }
}
