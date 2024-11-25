using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public VeController()
        {
            _db = new FilmsDbContext();
        }

        // Lấy danh sách tất cả vé
        [HttpGet(Name = "GetVe")]
        public async Task<IActionResult> GetVe()
        {
            try
            {
                var ve = await _db.Ves.ToListAsync();

                return Ok(ve);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost(Name = "ThemVe")]
        public async Task<IActionResult> AddVe([FromBody] Ve dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var ve = await _db.Ves.FindAsync(dto.MaVe);
                ve = dto;
                await _db.SaveChangesAsync();
                return Ok("Thêm vé thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut(Name = "Update")]
        public async Task<IActionResult> UpdateVe([FromBody] Ve dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var ve = await _db.Ves.FirstOrDefaultAsync(v => v.MaVe == dto.MaVe);

                if (ve == null)
                {
                    return NotFound("Không tìm thấy bản ghi cần cập nhật");
                }

                // Chỉ cập nhật các thuộc tính cụ th
                ve = dto;
                await _db.SaveChangesAsync();
                return Ok("Cập nhật vé thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
