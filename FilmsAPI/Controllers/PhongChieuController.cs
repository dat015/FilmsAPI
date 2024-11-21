using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongChieuController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public PhongChieuController(FilmsmanageDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetPhongChieu")]
        public async Task<IActionResult> GetPhongChieu()
        {
            try
            {
                var phongChieu = await _db.PhongChieus.ToListAsync();
                return Ok(phongChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost(Name = "AddPhongChieu")]
        public async Task<IActionResult> AddPhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenPhongChieu))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên phòng chiếu không được để trống");
            }

            try
            {
                var phongChieu = new PhongChieu
                {
                    TenPhongChieu = dto.TenPhongChieu,
                };

                _db.PhongChieus.Add(phongChieu);
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut(Name = "UpdatePhongChieu")]
        public async Task<IActionResult> UpdatePhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenPhongChieu))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên phòng chiếu không được để trống");
            }

            try
            {
                var phongChieu = await _db.PhongChieus.FindAsync(dto.IdPhongChieu);
                if (phongChieu == null)
                {
                    return NotFound("Không tìm thấy phòng chiếu");
                }

                phongChieu.TenPhongChieu = dto.TenPhongChieu;
                await _db.SaveChangesAsync();
                return Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
