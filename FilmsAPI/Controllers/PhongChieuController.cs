using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongChieuController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;  
        public PhongChieuController()
        {
            _db = new FilmsmanageDbContext();
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
                return BadRequest(ex.Message);
            }
        }

    }
}
