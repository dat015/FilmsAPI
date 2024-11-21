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

        public VeController(FilmsmanageDbContext db)
        {
<<<<<<< HEAD
            _db = db;
=======
            _db = new FilmsDbContext();
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
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

        // Thêm vé mới
        [HttpPost(Name = "ThemVe")]
        public async Task<IActionResult> AddVe([FromBody] Ve dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
<<<<<<< HEAD
                var ve = new Ve
                {
                    SoGhe = dto.SoGhe,
                    DonGia = dto.DonGia,
                    MaXuatChieu = dto.MaXuatChieu,
                    // Thêm các thuộc tính khác nếu cần
                };

                _db.Ves.Add(ve);
=======
                var ve = await _db.Ves.FindAsync(dto.MaVe);
                ve = dto;
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
                await _db.SaveChangesAsync();
                return Ok("Thêm vé thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Cập nhật vé
        [HttpPut(Name = "UpdateVe")]
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

                // Chỉ cập nhật các thuộc tính cụ thể
                ve.SoGhe = dto.SoGhe;
                ve.DonGia = dto.DonGia;
                ve.MaXuatChieu = dto.MaXuatChieu;

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
