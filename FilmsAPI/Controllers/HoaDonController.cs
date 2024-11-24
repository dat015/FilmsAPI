using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public HoaDonController()
        {
            _db = new FilmsDbContext();
        }

        // GET: api/HoaDon
        [HttpGet]
        public async Task<IActionResult> GetAllHoaDon()
        {
            try
            {
                var hoaDons = await _db.HoaDons
                    .Include(h => h.MaNvNavigation) // Kết hợp với thông tin nhân viên
                    .ToListAsync();

                return Ok(hoaDons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi lấy dữ liệu: " + ex.Message });
            }
        }

        // GET: api/HoaDon/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHoaDonById(int id)
        {
            try
            {
                var hoaDon = await _db.HoaDons
                    .Include(h => h.MaNvNavigation) // Kết hợp với thông tin nhân viên
                    .Include(h => h.ChiTietHoaDons) // Kết hợp với chi tiết hóa đơn
                    .FirstOrDefaultAsync(h => h.MaHd == id);

                if (hoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy hóa đơn với ID " + id });
                }

                return Ok(hoaDon);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi lấy dữ liệu: " + ex.Message });
            }
        }

        // POST: api/HoaDon
        [HttpPost]
        public async Task<IActionResult> CreateHoaDon([FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null || hoaDon.MaNv == 0 || hoaDon.TongTien <= 0)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                _db.HoaDons.Add(hoaDon);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetHoaDonById), new { id = hoaDon.MaHd }, hoaDon);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi thêm mới hóa đơn: " + ex.Message });
            }
        }

        // PUT: api/HoaDon/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHoaDon(int id, [FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null || id != hoaDon.MaHd)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                var existingHoaDon = await _db.HoaDons.FindAsync(id);

                if (existingHoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy hóa đơn cần cập nhật" });
                }

                existingHoaDon.TongTien = hoaDon.TongTien;
                existingHoaDon.MaNv = hoaDon.MaNv;
                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi cập nhật hóa đơn: " + ex.Message });
            }
        }

        // DELETE: api/HoaDon/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon(int id)
        {
            try
            {
                var hoaDon = await _db.HoaDons.FindAsync(id);

                if (hoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy hóa đơn cần xóa" });
                }

                _db.HoaDons.Remove(hoaDon);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Xóa hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi khi xóa hóa đơn: " + ex.Message });
            }
        }
    }
}
