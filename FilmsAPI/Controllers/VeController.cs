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
                var ve = await _db.Ves
                    .Include(v => v.MaLoaiVeNavigation)
                    .Include(v => v.MaGheNavigation)
                    .Include(v => v.MaXuatChieuNavigation)
                    .ToListAsync();
                return Ok(ve);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetVeTheoXuatChieu/{id}")]
        public async Task<IActionResult> GetVe(int id)
        {
            try
            {
                var ve = await _db.Ves.Where(v => v.MaXuatChieu == id)
                    .Include(v => v.MaGheNavigation)
                    .ThenInclude(g => g.MaLoaiGheNavigation)
                    .ToListAsync();
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
                _db.Ves.Add(dto);
                await _db.SaveChangesAsync();
                return Ok(new { Message = "Thêm vé thành công" });
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
                return BadRequest(new { Message = "Cung cấp đủ dữ liệu" });
            }

            try
            {
                var ve = await _db.Ves.FirstOrDefaultAsync(v => v.MaVe == dto.MaVe);

                if (ve == null)
                {
                    return NotFound(new { Message = "Không tìm thấy bản ghi cần cập nhật" });
                }

                // Chỉ cập nhật các thuộc tính cụ th
                ve.MaLoaiVe = dto.MaLoaiVe;
                ve.GiaVe = dto.GiaVe;
                ve.TrangThai = dto.TrangThai;
                ve.MaGhe = dto.MaGhe;
                ve.MaXuatChieu = dto.MaXuatChieu;
                await _db.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật vé thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
