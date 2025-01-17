﻿using FilmsAPI.Filters;
using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FilmsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RoleAuthorizationFilter("Admin")]

    public class DangPhimController : ControllerBase
    {


        private readonly FilmsDbContext _db;

        // Dependency Injection cho DbContext
        public DangPhimController()
        {
            _db = new FilmsDbContext();
        }

        // GET: /DangPhim
        [HttpGet(Name = "GetDangPhim")]
        public async Task<IActionResult> GetDangPhim()
        {
            try
            {
                var dangPhim = await _db.DangPhims.
                    Include(p => p.MaManHinhNavigation)
                    .ToListAsync();
                return Ok(dangPhim);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = "Không tìm thấy dữ liệu dạng phim." });

            }
        }

        // PUT: /DangPhim   
        [HttpPut(Name = "UpdateDangPhim")]
        public async Task<IActionResult> Update([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống." });
            }

            try
            {
                var dangPhim = await _db.DangPhims
                    .FirstOrDefaultAsync(dp => dp.MaDangPhim == dto.MaDangPhim);

                if (dangPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy bản ghi cần cập nhật." });
                }

                // Kiểm tra xem tên dạng phim mới đã tồn tại trong cơ sở dữ liệu chưa
                var existingDangPhim = await _db.DangPhims
                    .FirstOrDefaultAsync(dp => dp.TenDangPhim == dto.TenDangPhim && dp.MaDangPhim != dto.MaDangPhim);

                if (existingDangPhim != null)
                {
                    return BadRequest(new { message = "Tên dạng phim đã tồn tại." });
                }

                // Cập nhật các trường dữ liệu
                dangPhim.MaManHinh = dto.MaManHinh;
                dangPhim.TenDangPhim = dto.TenDangPhim;

                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi cập nhật.", error = ex.Message });
            }
        }

        // POST: /DangPhim
        [HttpPost(Name = "CreateDangPhim")]
        public async Task<IActionResult> Create([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống." });
            }

            try
            {
                // Kiểm tra xem tên dạng phim đã tồn tại chưa
                var existingDangPhim = await _db.DangPhims
                    .FirstOrDefaultAsync(dp => dp.TenDangPhim == dto.TenDangPhim);

                if (existingDangPhim != null)
                {
                    return BadRequest(new { message = "Tên dạng phim đã tồn tại." });
                }

                var dangPhim = new DangPhim
                {
                    TenDangPhim = dto.TenDangPhim,
                    MaManHinh = dto.MaManHinh
                };

                _db.DangPhims.Add(dangPhim);
                await _db.SaveChangesAsync();


                // Đảm bảo phản hồi rõ ràng
                return CreatedAtAction("GetDangPhim", new { id = dangPhim.MaDangPhim }, new { message = "Thêm mới thành công." });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Lỗi khi thêm mới dạng phim.", error = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi thêm mới dạng phim.", error = ex.Message });

            }
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("Vui lòng chọn 1 dạng phim để xóa!");
            }
            try
            {
                var dangPhim = await _db.DangPhims.FindAsync(id);
                if (dangPhim == null)
                {
                    return BadRequest("Dạng phim này không tồn tại");
                }

                _db.DangPhims.Remove(dangPhim);
                await _db.SaveChangesAsync();

                return Ok("Xóa thành công!");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}
