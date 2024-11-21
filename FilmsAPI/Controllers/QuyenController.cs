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
        private readonly FilmsDbContext _db;

        public QuyenController(FilmsmanageDbContext db)
        {
<<<<<<< HEAD
            _db = db;
=======
            _db = new FilmsDbContext();
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
        }

        // Lấy danh sách quyền
        [HttpGet(Name = "GetQuyen")]
        public async Task<IActionResult> GetQuyen()
        {
            try
            {
<<<<<<< HEAD
                var quyenList = await _db.Quyens.ToListAsync();
                return Ok(quyenList);
=======
                var quyen = await _db.Quyens.ToListAsync();
                return Ok(quyen);
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy dữ liệu: {ex.Message}");
            }
        }

<<<<<<< HEAD
        // Thêm quyền mới
=======
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
        [HttpPost(Name = "AddQuyen")]
        public async Task<IActionResult> AddQuyen([FromBody] Quyen dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenQuyen))
            {
<<<<<<< HEAD
                return BadRequest("Cung cấp đủ dữ liệu và tên quyền không được để trống.");
=======
                return BadRequest("Dữ liệu không hợp lệ");
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
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
<<<<<<< HEAD
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
=======

                return CreatedAtAction(nameof(AddQuyen), new { id = quyen.MaQuyen }, quyen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi: " + ex.Message);
            }
        }


        [HttpPut(Name = "UpdateQuyen")]
        public async Task<IActionResult> UpdateQuyen([FromBody] Quyen dto)
        {
            if (dto == null || dto.MaQuyen <= 0)
            {
                return BadRequest("ID quyền không hợp lệ");
            }

            if (string.IsNullOrWhiteSpace(dto.TenQuyen))
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên quyền không được để trống.");
            }

            try
            {
                // Tìm kiếm quyền theo mã
                var quyen = await _db.Quyens.FirstOrDefaultAsync(q => q.MaQuyen == dto.MaQuyen);

                if (quyen == null)
                {
<<<<<<< HEAD
                    return NotFound("Không tìm thấy quyền cần cập nhật.");
=======
                    return NotFound("Không tìm thấy quyền cần cập nhật");
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
                }

                // Cập nhật thuộc tính
                quyen.TenQuyen = dto.TenQuyen;
                await _db.SaveChangesAsync();
<<<<<<< HEAD
                return Ok("Cập nhật quyền thành công.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi cơ sở dữ liệu: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi không xác định: {ex.Message}");
=======

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi: " + ex.Message);
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
            }
        }

    }
}
