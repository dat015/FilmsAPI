using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FilmsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DangPhimController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        // Dependency Injection cho DbContext
        public DangPhimController()
        {
            _db = new FilmsDbContext();
        }

        // GET: /DangPhim
        [HttpGet(Name = "GetDangPhim")]
        public async Task<IActionResult> GetDangPhim()
        {
            try
            {
                //day la cmt
                var dangPhims = await _db.DangPhims
                    .Include(d => d.Phims)
                    .Include(d => d.MaManHinhNavigation)
                    .ToListAsync();

                if (dangPhims == null || !dangPhims.Any())
                {
                    return NotFound(new { message = "Không có dữ liệu dạng phim." });
                }

                return Ok(dangPhims);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi truy vấn cơ sở dữ liệu.", error = ex.Message });
            }
        }

        // PUT: /DangPhim
        [HttpPut(Name = "UpdateDangPhim")]
        public async Task<IActionResult> Update([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống." });
            }

            try
            {
                var dangPhim = await _db.DangPhims
                    .FirstOrDefaultAsync(dp => dp.MaDangPhim == dto.MaDangPhim);

                if (dangPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy bản ghi cần cập nhật." });
                }

                // Cập nhật các trường dữ liệu
                dangPhim.TenDangPhim = dto.TenDangPhim;
                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi cập nhật.", error = ex.Message });
            }
        }

        // POST: /DangPhim
        [HttpPost(Name = "CreateDangPhim")]
        public async Task<IActionResult> Create([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống." });
            }

            try
            {
                var existingDangPhim = await _db.DangPhims
                    .Where(d => d.TenDangPhim.ToLower() == dto.TenDangPhim.ToLower())
                    .FirstOrDefaultAsync();

                if (existingDangPhim != null)
                {
                    return BadRequest(new { message = "Dạng phim này đã tồn tại." });
                }

                var dangPhim = new DangPhim
                {
                    TenDangPhim = dto.TenDangPhim,
                    MaManHinh = dto.MaManHinh // Thiết lập ManHinh cho DangPhim
                };

                _db.DangPhims.Add(dangPhim);
                await _db.SaveChangesAsync();

                return CreatedAtAction("GetDangPhim", new { id = dangPhim.MaDangPhim }, dangPhim);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi thêm mới dạng phim.", error = ex.Message });
            }
        }
    }
}
