using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinhTrangController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public TinhTrangController(FilmsmanageDbContext db)
        {
            _db = db;
        }

        // Lấy danh sách tình trạng
        [HttpGet(Name = "GetTinhTrang")]
        public async Task<IActionResult> GetTinhTrang()
        {
            try
            {
                var tinhTrang = await _db.TinhTrangs.ToListAsync();
                if (tinhTrang == null || tinhTrang.Count == 0)
                {
                    return NotFound("Không có dữ liệu tình trạng nào.");
                }
                return Ok(tinhTrang);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        // Thêm tình trạng mới
        [HttpPost(Name = "AddTinhTrang")]
        public async Task<IActionResult> AddTinhTrang([FromBody] TinhTrang dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu.");
            }

            try
            {
                // Kiểm tra nếu tình trạng đã tồn tại (nếu cần)
                var existingTinhTrang = await _db.TinhTrangs
                    .FirstOrDefaultAsync(t => t.TenTinhTrang == dto.TenTinhTrang);
                if (existingTinhTrang != null)
                {
                    return Conflict("Tình trạng này đã tồn tại.");
                }

                var tinhTrang = new TinhTrang
                {
                    TenTinhTrang = dto.TenTinhTrang
                };

                _db.TinhTrangs.Add(tinhTrang);
                await _db.SaveChangesAsync();
                return CreatedAtAction("GetTinhTrang", new { id = tinhTrang.MaTinhTrang }, tinhTrang);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi thêm tình trạng: {ex.Message}");
            }
        }

        // Cập nhật tình trạng
        [HttpPut(Name = "UpdateTinhTrang")]
        public async Task<IActionResult> UpdateTinhTrang([FromBody] TinhTrang dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenTinhTrang))
            {
                return BadRequest("Tên tình trạng không được để trống.");
            }

            try
            {
                var tinhTrang = await _db.TinhTrangs.FirstOrDefaultAsync(dp => dp.MaTinhTrang == dto.MaTinhTrang);

                if (tinhTrang == null)
                {
                    return NotFound($"Không tìm thấy tình trạng với mã {dto.MaTinhTrang}.");
                }

                tinhTrang.TenTinhTrang = dto.TenTinhTrang;

                await _db.SaveChangesAsync();
                return Ok("Cập nhật tình trạng thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi cập nhật tình trạng: {ex.Message}");
            }
        }
    }
}
