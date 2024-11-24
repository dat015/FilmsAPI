﻿using FilmsAPI.Models;
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

        public QuyenController()
        {
            _db = new FilmsDbContext();
        }

        [HttpGet(Name = "GetQuyen")]
<<<<<<< HEAD
        public ActionResult GetQuyen()
=======
        public async Task<IActionResult> GetQuyen()
>>>>>>> 929d576b2d3e51fdab03da8214fa51ca1cd8d022
        {
            try
            {
                var quyen = await _db.Quyens.ToListAsync();
                return Ok(quyen);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost(Name = "AddQuyen")]
        public async Task<IActionResult> AddQuyen([FromBody] Quyen dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenQuyen))
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            try
            {
                var quyen = new Quyen
                {
                    TenQuyen = dto.TenQuyen
                };

                _db.Quyens.Add(quyen);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(AddQuyen), new { id = quyen.MaQuyen }, quyen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi: " + ex.Message);
            }
        }

<<<<<<< HEAD
=======

>>>>>>> 929d576b2d3e51fdab03da8214fa51ca1cd8d022
        [HttpPut(Name = "UpdateQuyen")]
        public async Task<IActionResult> UpdateQuyen([FromBody] Quyen dto)
        {
            if (dto == null || dto.MaQuyen <= 0)
            {
                return BadRequest("ID quyền không hợp lệ");
            }

            if (string.IsNullOrWhiteSpace(dto.TenQuyen))
            {
                return BadRequest("Tên quyền không được để trống");
            }

            try
            {
                var quyen = await _db.Quyens.FirstOrDefaultAsync(dp => dp.MaQuyen == dto.MaQuyen);

                if (quyen == null)
                {
                    return NotFound("Không tìm thấy quyền cần cập nhật");
                }

                quyen.TenQuyen = dto.TenQuyen;
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi: " + ex.Message);
            }
        }

    }
}
