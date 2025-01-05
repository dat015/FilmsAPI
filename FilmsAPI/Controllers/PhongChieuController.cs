
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;
using System.Reflection.PortableExecutable;
using FilmsAPI.Filters;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

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

        [HttpPost(Name = "AddPhongChieu")]
        public async Task<IActionResult> AddPhongChieu([FromBody] PhongChieu dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenPhongChieu))
            {
                return BadRequest("Cung cấp đủ dữ liệu và tên phòng chiếu không được để trống");
            }

            try
            {
                // Kiểm tra tên phòng chiếu đã tồn tại
                var existingPhongChieu = await _db.PhongChieus
                    .FirstOrDefaultAsync(p => p.TenPhongChieu == dto.TenPhongChieu);

                if (existingPhongChieu != null)
                {
                    return BadRequest(new { message = "Tên phòng chiếu đã tồn tại." });
                }
                var phongChieu = new PhongChieu
                {
                    TenPhongChieu = dto.TenPhongChieu,
                    SoGhe = dto.SoGhe,
                    SoGheMotHang = dto.SoGheMotHang,
                    MaManHinh = dto.MaManHinh
                };

                _db.PhongChieus.Add(phongChieu);
                await _db.SaveChangesAsync();
                return CreatedAtAction("GetPhongChieu", new { id = phongChieu.MaPhongChieu }, new { message = "Thêm mới thành công." });
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
                var phongChieu = await _db.PhongChieus
                    .FirstOrDefaultAsync(dp => dp.MaPhongChieu == dto.MaPhongChieu);

                if (phongChieu == null)
                {
                    return NotFound("Không tìm thấy phòng chiếu");
                }

                var existingPhongChieu = await _db.PhongChieus
                    .FirstOrDefaultAsync(dp => dp.TenPhongChieu == dto.TenPhongChieu && dp.MaPhongChieu != dto.MaPhongChieu);

                if (existingPhongChieu != null)
                {
                    return BadRequest(new { message = "Tên phòng chiếu đã tồn tại." });
                }


                phongChieu.TenPhongChieu = dto.TenPhongChieu;
                phongChieu.SoGhe = dto.SoGhe;
                phongChieu.SoGheMotHang = dto.SoGheMotHang;
                phongChieu.MaManHinhNavigation = new ManHinh
                {
                    TenManHinh = dto.MaManHinhNavigation.TenManHinh
                };
                await _db.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0)
            {
                return BadRequest("Vui lòng chọn một phòng chiếu để xóa!");
            }
            try
            {
                var phongChieu = await _db.PhongChieus.FindAsync(id);
                if(phongChieu == null)
                {
                    return BadRequest("Phòng chiếu không tồn tại!");
                }

                _db.PhongChieus.Remove(phongChieu);
                await _db.SaveChangesAsync();
                return Ok("Xóa thành công!");
            }
            catch( Exception ex)
            {
                return BadRequest("Không thể xóa. Lỗi:  " +ex.Message);

            }
        }
    }
}
