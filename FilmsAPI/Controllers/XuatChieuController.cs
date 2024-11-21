using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatChieuController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public XuatChieuController(FilmsmanageDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetXuatChieu")]
        public async Task<IActionResult> GetXuatChieu()
        {
            try
            {
                var xuatChieu = await _db.XuatChieus.ToListAsync();
                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(Name = "AddXuatChieu")]
        public async Task<IActionResult> AddXuatChieu([FromBody] XuatChieu dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                _db.XuatChieus.Add(dto);
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut(Name = "UpdateXuatChieu")]
        public async Task<IActionResult> UpdateXuatChieu([FromBody] XuatChieu dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var xuatChieu = await _db.XuatChieus.FirstOrDefaultAsync(v => v.MaXuatChieu == dto.MaXuatChieu);

                if (xuatChieu == null)
                {
                    return NotFound("Không tìm thấy bản ghi cần cập nhật");
                }

                xuatChieu.MaPhim = dto.MaPhim;
                xuatChieu.GioChieu = dto.GioChieu;
                xuatChieu.PhutChieu = dto.PhutChieu;
                // Các thuộc tính khác nếu cần...

                await _db.SaveChangesAsync();
                return Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

