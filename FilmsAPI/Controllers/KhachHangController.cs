using FilmsAPI.Filters;
using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[RoleAuthorizationFilter("Admin")]

    public class KhachHangController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public KhachHangController()
        {
            _db = new FilmsDbContext();
        }
        // GET: api/KhachHang
        [HttpGet]
        public async Task<IActionResult> GetAllKhachHang()
        {
            try
            {
                var khachHangs = await _db.KhachHangs.ToListAsync();

                if (khachHangs == null || khachHangs.Count == 0)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng nào." });
                }

                return Ok(khachHangs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/KhachHang/{maKh}
        [HttpGet("{maKh}")]
        public async Task<IActionResult> GetKhachHangById(int maKh)
        {
            try
            {
                var khachHang = await _db.KhachHangs
                    .FirstOrDefaultAsync(kh => kh.MaKh == maKh);

                if (khachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng với MaKh " + maKh });
                }

                return Ok(khachHang);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/KhachHang
        [HttpPost]
        public async Task<IActionResult> CreateKhachHang([FromBody] KhachHang dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                _db.KhachHangs.Add(dto);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetKhachHangById), new { maKh = dto.MaKh }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/KhachHang
        [HttpPut]
        public async Task<IActionResult> UpdateKhachHang([FromBody] KhachHang dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingKhachHang = await _db.KhachHangs
                    .FirstOrDefaultAsync(kh => kh.MaKh == dto.MaKh);

                if (existingKhachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng với MaKh " + dto.MaKh });
                }

                // Cập nhật thông tin khách hàng
                existingKhachHang.TenKh = dto.TenKh;
                existingKhachHang.Sdt = dto.Sdt;
                existingKhachHang.Email = dto.Email;
                existingKhachHang.CCCD = dto.CCCD;
                existingKhachHang.NgaySinh = dto.NgaySinh;
                existingKhachHang.DiaChi = dto.DiaChi;
                existingKhachHang.DiemTichluy = dto.DiemTichluy;   

                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật khách hàng thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/KhachHang/{maKh}
        [HttpDelete("{maKh}")]
        public async Task<IActionResult> DeleteKhachHang(int maKh)
        {
            try
            {
                var khachHang = await _db.KhachHangs
                    .FirstOrDefaultAsync(kh => kh.MaKh == maKh);

                if (khachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng với MaKh " + maKh });
                }

                _db.KhachHangs.Remove(khachHang);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Xóa khách hàng thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
