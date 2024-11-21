using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.DTO;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GheController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public GheController()
        {
            _db = new FilmsDbContext();
        }

        // GET: api/Ghe/{maPhong}
        [HttpGet("{maPhong}", Name = "GetDanhSachGhe")]
        public async Task<IActionResult> GetDanhSachGhe(int maPhong)
        {
            try
            {
                var ds = await _db.Ghes
                    .Where(d => d.MaPhong == maPhong)
                    .Include(g => g.MaLoaiGheNavigation)  // Bao gồm thông tin loại ghế
                    .Include(g => g.MaPhongNavigation)   // Bao gồm thông tin phòng chiếu
                    .ToListAsync();

                if (ds == null || !ds.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy ghế nào cho mã phòng này." });
                }

                return Ok(ds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi truy vấn cơ sở dữ liệu", Error = ex.Message });
            }
        }

        // PUT: api/Ghe
        [HttpPut(Name = "UpdateGhe")]
        public async Task<IActionResult> UpdateGhe([FromBody] Ghe dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                // Tìm ghế theo MaGhe (để cập nhật)
                var ghe = await _db.Ghes.FindAsync(dto.MaGhe);
                if (ghe == null)
                {
                    return NotFound(new { Message = "Không tìm thấy ghế với ID được cung cấp." });
                }

                ghe.MaPhong = dto.MaPhong;
                ghe.TrangThai = dto.TrangThai;  // Cập nhật trạng thái ghế
                ghe.MaLoaiGhe = dto.MaLoaiGhe;

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Cập nhật ghế thành công!", UpdatedGhe = ghe });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật ghế.", Error = ex.Message });
            }
        }

        // POST: api/Ghe
        [HttpPost(Name = "AddGhe")]
        public async Task<IActionResult> AddGhe([FromBody] Ghe dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ!" });
            }

            try
            {
                _db.Ghes.Add(dto);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetDanhSachGhe", new { maPhong = dto.MaPhong }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi thêm ghế.", Error = ex.Message });
            }
        }
    }
}
