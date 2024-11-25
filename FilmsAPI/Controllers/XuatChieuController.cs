using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatChieuController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public XuatChieuController()
        {
            _db = new FilmsDbContext();
        }

        [HttpGet(Name = "GetXuatChieu")]
        public ActionResult GetXuatChieu()
        {
            try
            {
                var xuatChieu = _db.XuatChieus.ToListAsync();
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
                xuatChieu = dto;

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

