using FilmsAPI.Filters;
using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

    public class CTHDController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public CTHDController()
        {
            _db = new FilmsDbContext();
        }

        // GET: api/CTHD/{maHd}
        [HttpGet("{maHd}")]
        public async Task<IActionResult> GetChiTietHoaDonById(int maHd)
        {
            try
            {
                var chiTietHoaDons = await _db.ChiTietHoaDons
                    .Include(ct => ct.MaVeNavigation)  // Bao gồm thông tin vé
                    .Where(ct => ct.MaHd == maHd)      // Lọc theo MaHd
                    .ToListAsync();

                if (chiTietHoaDons == null || !chiTietHoaDons.Any())
                {
                    return NotFound(new { message = "Không tìm thấy chi tiết hóa đơn với MaHd " + maHd });
                }

                return Ok(chiTietHoaDons); // Trả về danh sách chi tiết hóa đơn
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // POST: api/CTHD
        [HttpPost]
        public async Task<IActionResult> CreateChiTietHoaDon([FromBody] ChiTietHoaDon dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                // Tính toán lại ThanhTien (Số lượng * Giá vé)
                dto.ThanhTien = dto.SoLuong * dto.MaVeNavigation.GiaVe;

                _db.ChiTietHoaDons.Add(dto);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetChiTietHoaDonById), new { maHd = dto.MaHd }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/CTHD
        [HttpPut]
        public async Task<IActionResult> UpdateChiTietHoaDon([FromBody] ChiTietHoaDon dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingChiTietHoaDon = await _db.ChiTietHoaDons
                    .FirstOrDefaultAsync(ct => ct.MaHd == dto.MaHd && ct.MaVe == dto.MaVe);

                if (existingChiTietHoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy chi tiết hóa đơn với MaHd và MaVe đã cho." });
                }

                // Cập nhật số lượng và tính lại Thành Tiền
                existingChiTietHoaDon.SoLuong = dto.SoLuong;
                existingChiTietHoaDon.ThanhTien = dto.SoLuong * existingChiTietHoaDon.MaVeNavigation.GiaVe;

                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/CTHD/{maHd}/{maVe}
        [HttpDelete("{maHd}/{maVe}")]
        public async Task<IActionResult> DeleteChiTietHoaDon(int maHd, int maVe)
        {
            try
            {
                var chiTietHoaDon = await _db.ChiTietHoaDons
                    .FirstOrDefaultAsync(ct => ct.MaHd == maHd && ct.MaVe == maVe);

                if (chiTietHoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy chi tiết hóa đơn với MaHd và MaVe đã cho." });
                }

                _db.ChiTietHoaDons.Remove(chiTietHoaDon);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Xóa thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
