using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuyenController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public QuyenController(FilmsmanageDbContext db)
        {
            _db = db;
        }

        // Lấy danh sách quyền
        [HttpGet(Name = "GetQuyen")]
        public async Task<IActionResult> GetQuyen()
        {
            try
            {
                var quyenList = await _db.Quyens.ToListAsync();
                return Ok(quyenList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }

        // Thêm quyền mới
        [HttpPost(Name = "AddQuyen")]
        public async Task<IActionResult> AddQuyen([FromBody] Quyen dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenQuyen))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên quyền không được để trống.");
            }

            try
            {
                // Kiểm tra tên quyền đã tồn tại hay chưa
                var existingQuyen = await _db.Quyens.FirstOrDefaultAsync(q => q.TenQuyen == dto.TenQuyen);
                if (existingQuyen != null)
                {
                    return BadRequest("Tên quyền đã tồn tại.");
                }

                // Tạo đối tượng mới
                var quyen = new Quyen
                {
                    TenQuyen = dto.TenQuyen
                };

                _db.Quyens.Add(quyen);
                await _db.SaveChangesAsync();
                return Ok("Thêm quyền thành công.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi cơ sở dữ liệu: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi không xác định: {ex.Message}");
            }
        }

        // Cập nhật quyền
        [HttpPut(Name = "UpdateQuyen")]
        public async Task<IActionResult> UpdateQuyen([FromBody] Quyen dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenQuyen))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên quyền không được để trống.");
            }

            try
            {
                // Tìm kiếm quyền theo mã
                var quyen = await _db.Quyens.FirstOrDefaultAsync(q => q.MaQuyen == dto.MaQuyen);

                if (quyen == null)
                {
                    return NotFound("Không tìm thấy quyền cần cập nhật.");
                }

                // Cập nhật thuộc tính
                quyen.TenQuyen = dto.TenQuyen;

                await _db.SaveChangesAsync();
                return Ok("Cập nhật quyền thành công.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi cơ sở dữ liệu: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}
