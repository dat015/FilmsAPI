<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongChieuController : ControllerBase
    {
<<<<<<< HEAD
        private readonly FilmsmanageDbContext _db;

        public PhongChieuController(FilmsmanageDbContext db)
        {
            _db = db;
=======
        private readonly FilmsDbContext _db;
        public PhongChieuController()
        {
            _db = new FilmsDbContext();
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
        }

        [HttpGet(Name = "GetPhongChieu")]
        public async Task<IActionResult> GetPhongChieu()
        {
            try
            {
                var phongChieu = await _db.PhongChieus.ToListAsync();
                return Ok(phongChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost(Name = "AddPhongChieu")]
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
