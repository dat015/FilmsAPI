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
        public DangPhimController()
        {
            _db = new FilmsmanageDbContext();
        }
        [HttpGet(Name = "GetDangPhim")]

        public async Task<IActionResult> GetDangPhim()
        {
            try
            {
                var DangPhims = await _db.DangPhims.ToListAsync();
                return Ok(DangPhims);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [HttpPost]
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
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPut(Name = "Create")]
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

                return Ok(new { message = "Thêm mới thành công", dangPhimId = dangPhim.MaDangPhim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }




    }
}
