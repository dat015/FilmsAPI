
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongChieuController : ControllerBase
    {

        private readonly FilmsDbContext _db;
        public PhongChieuController()
        {
            _db = new FilmsDbContext();
        }

        [HttpGet(Name = "GetPhongChieu")]
        public async Task<IActionResult> GetPhongChieu()
        {
            try
            {
                var phongChieu = await _db.PhongChieus.
                    Include(p => p.MaManHinhNavigation)
                    .ToListAsync();
                return Ok(phongChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

<<<<<<< HEAD
        [HttpPost(Name = "AddPhongChieu")]
=======
        [HttpPut(Name = "AddPhongChieu")]
>>>>>>> dd8fd136c5fa2df690d53a99ce83d01fe90cbf32
        public async Task<IActionResult> AddPhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenPhongChieu))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên phòng chiếu không được để trống");
            }

            try
            {
                var phongChieu = new PhongChieu
                {
                    TenPhongChieu = dto.TenPhongChieu,
                    SoGhe = dto.SoGhe,
                    SoGheMotHang = dto.SoGheMotHang,
                };

                _db.PhongChieus.Add(phongChieu);
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut(Name = "UpdatePhongChieu")]
        public async Task<IActionResult> UpdatePhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenPhongChieu))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên phòng chiếu không được để trống");
            }

            try
            {
                var phongChieu = await _db.PhongChieus.FindAsync(dto.MaPhongChieu);
                if (phongChieu == null)
                {
                    return NotFound("Không tìm thấy phòng chiếu");
                }

                phongChieu.TenPhongChieu = dto.TenPhongChieu;
                await _db.SaveChangesAsync();
                return Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
