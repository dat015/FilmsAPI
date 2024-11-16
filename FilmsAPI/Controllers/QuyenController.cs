using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuyenController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public QuyenController()
        {
            _db = new FilmsmanageDbContext();
        }

        [HttpGet(Name = "GetQuyen")]
        public IActionResult GetQuyen()
        {
            try
            {
                var quyen = _db.Quyens.ToList();
                return Ok(quyen);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut(Name = "AddQuyen")]
        public async Task<IActionResult> AddQuyen([FromBody] Quyen dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var quyen = new Quyen
                {
                    TenQuyen = dto.TenQuyen
                };

                _db.Quyens.Add(quyen);
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "UpdateQuyen")]
        public async Task<IActionResult> UpdateQuyen([FromBody] Quyen dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenQuyen))
            {
                return BadRequest("Tên quyền không được để trống");
            }

            try
            {
                var quyen = await _db.Quyens.FirstOrDefaultAsync(dp => dp.MaQuyen == dto.MaQuyen);

                if (quyen == null)
                {
                    return NotFound("Không tìm thấy bản ghi cần cập nhật");
                }

                quyen.TenQuyen = dto.TenQuyen;

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
