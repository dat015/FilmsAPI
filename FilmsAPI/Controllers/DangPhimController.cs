using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DangPhimController : Controller
    {
        private readonly FilmsmanageDbContext _db;

        public DangPhimController(FilmsmanageDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetDangPhim")]
        public async Task<IActionResult> GetDangPhim()
        {
            try
            {
                var dangPhims = await _db.DangPhims.ToListAsync();
                if (dangPhims == null || dangPhims.Count == 0)
                {
                    return NotFound(new { message = "Không có dữ liệu dạng phim nào." });
                }
                return Ok(dangPhims);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // Cập nhật dạng phim
        [HttpPut(Name = "UpdateDangPhim")]
        public async Task<IActionResult> Update([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống" });
            }

            try
            {
                var dangPhim = await _db.DangPhims.FirstOrDefaultAsync(dp => dp.MaDangPhim == dto.MaDangPhim);
                if (dangPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy bản ghi cần cập nhật" });
                }

                dangPhim.TenDangPhim = dto.TenDangPhim;
                await _db.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi cập nhật: " + ex.Message });
            }
        }

        // Thêm mới dạng phim
        [HttpPost(Name = "CreateDangPhim")]
        public async Task<IActionResult> Create([FromBody] DangPhim dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống" });
            }

            try
            {
                var existingDangPhim = await _db.DangPhims
                    .Where(d => d.TenDangPhim.ToLower() == dto.TenDangPhim.ToLower())
                    .FirstOrDefaultAsync();

                if (existingDangPhim != null)
                {
                    return BadRequest(new { message = "Dạng phim này đã tồn tại" });
                }

                var dangPhim = new DangPhim
                {
                    TenDangPhim = dto.TenDangPhim
                };

                _db.DangPhims.Add(dangPhim);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDangPhim), new { id = dangPhim.MaDangPhim }, new { message = "Thêm mới thành công", dangPhimId = dangPhim.MaDangPhim });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi tạo mới: " + ex.Message });
            }
        }
    }
}
