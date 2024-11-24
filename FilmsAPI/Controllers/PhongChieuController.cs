using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongChieuController : ControllerBase
    {
        private readonly FilmsDbContext _db;
        public PhongChieuController()
        {
            _db = new FilmsDbContext();
        }

        [HttpGet(Name = "GetPhongChieu")]
        public async Task<IActionResult> GetPhongChieu()
        {
            try
            {
                var phongChieu = await _db.PhongChieus.
                    Include(p => p.MaManHinhNavigation)
                    .ToListAsync();
                return Ok(phongChieu);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut(Name = "AddPhongChieu")]
        public async Task<IActionResult> AddPhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }
            try
            {
                var phongChieu = new PhongChieu
                {
                    TenPhongChieu = dto.TenPhongChieu,
                    SoGhe = dto.SoGhe,
                    SoGheMotHang = dto.SoGheMotHang,
                };
                _db.PhongChieus.Add(phongChieu);
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(Name = "UpdatePhongChieu")]
        public async Task<IActionResult> UpdatePhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }
            try
            {
                var phongChieu = await _db.PhongChieus.FindAsync(dto.MaPhongChieu);
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
                return BadRequest(ex.Message);
            }
        }

    }
}
