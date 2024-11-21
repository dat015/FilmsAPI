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
                return NotFound();
            }
        }

        [HttpPut(Name = "ThemVe")]
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
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("api/Ve", Name = "UpdateVe")]
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

                ve = dto;

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
