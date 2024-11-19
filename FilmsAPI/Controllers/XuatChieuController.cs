using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XuatChieuController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public XuatChieuController()
        {
            _db = new FilmsmanageDbContext();
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
                return NotFound();
            }
        }

        [HttpPut(Name = "AddXuatChieu")]
        public async Task<IActionResult> AddXuatChieu([FromBody] XuatChieu dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var xuatChieu = await _db.XuatChieus.FindAsync(dto.MaXuatChieu);
                xuatChieu = dto;
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "UpdateXuatChieu")]
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
                return BadRequest(ex.Message);
            }
        }
    }
}
