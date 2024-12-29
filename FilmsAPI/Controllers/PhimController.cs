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
    public class PhimController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public PhimController()
        {
            _db = new FilmsDbContext();
        }

        // Lấy danh sách phim
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Console.WriteLine("Bắt đầu lấy danh sách phim...");
                var phim = _db.Phims
                    .Include(p => p.MaDangPhimNavigation) // Lấy thông tin dạng phim
                    .Include(p => p.TheLoaiCuaPhims)
                    .Include(p => p.XuatChieus)           // Lấy thông tin xuất chiếu
                    .ToList();
                Console.WriteLine("Lấy danh sách phim thành công.");

                return Ok(phim);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách phim: {ex.Message}");
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // Lấy phim theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Console.WriteLine($"Bắt đầu tìm phim với ID: {id}");
                var phim = await _db.Phims
                    .Include(p => p.MaDangPhimNavigation)
                    .Include(p => p.TheLoaiCuaPhims)
                    .Include(p => p.XuatChieus)
                    .FirstOrDefaultAsync(p => p.MaPhim == id);

                if (phim == null)
                {
                    Console.WriteLine("Không tìm thấy phim.");
                    return NotFound(new { Message = "Không tìm thấy phim!" });
                }

                Console.WriteLine("Tìm thấy phim.");
                return Ok(phim);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm phim: {ex.Message}");
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // Thêm phim mới
        [HttpPost(Name = "AddPhim")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddPhim([FromBody] Phim dto)
        {
            if (dto == null)
            {
                Console.WriteLine("Dữ liệu không hợp lệ.");
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                Console.WriteLine("Bắt đầu thêm phim mới...");
                var phim = new Phim
                {
                    TenPhim = dto.TenPhim,
                    GhiChu = dto.GhiChu,
                    Mota = dto.Mota,
                    NoiDung = dto.NoiDung,
                    ThoiLuong = dto.ThoiLuong,
                    NgayKc = dto.NgayKc,
                    NgayKt = dto.NgayKt,
                    AnhBia = dto.AnhBia,
                    TenDaoDien = dto.TenDaoDien,
                    MaDangPhim = dto.MaDangPhim,
                    DoTuoi = dto.DoTuoi
                };

                await _db.Phims.AddAsync(phim);
                await _db.SaveChangesAsync();
                Console.WriteLine("Thêm phim mới thành công.");

                return Ok(new { Message = "Thêm phim thành công", phim = phim });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm phim: {ex.Message}");
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // Cập nhật phim
        [HttpPut(Name = "UpdatePhim")]
        public async Task<IActionResult> UpdatePhim([FromBody] Phim dto)
        {
            if (dto == null)
            {
                Console.WriteLine("Dữ liệu cập nhật không hợp lệ.");
                return BadRequest(new { Message = "Cung cấp đủ dữ liệu!" });
            }

            try
            {
                Console.WriteLine($"Bắt đầu cập nhật phim với ID: {dto.MaPhim}");
                var phim = await _db.Phims.FindAsync(dto.MaPhim);

                if (phim == null)
                {
                    Console.WriteLine("Không tìm thấy phim để cập nhật.");
                    return NotFound(new { Message = "Phim không tồn tại" });
                }

                phim.TenPhim = dto.TenPhim ?? phim.TenPhim;
                phim.GhiChu = dto.GhiChu ?? phim.GhiChu;
                phim.Mota = dto.Mota ?? phim.Mota;
                phim.NoiDung = dto.NoiDung ?? phim.NoiDung;
                phim.ThoiLuong = dto.ThoiLuong != 0 ? dto.ThoiLuong : phim.ThoiLuong;
                phim.NgayKc = dto.NgayKc != DateOnly.MinValue ? dto.NgayKc : phim.NgayKc;
                phim.NgayKt = dto.NgayKt != DateOnly.MinValue ? dto.NgayKt : phim.NgayKt;
                phim.AnhBia = dto.AnhBia ?? phim.AnhBia;
                phim.TenDaoDien = dto.TenDaoDien ?? phim.TenDaoDien;
                phim.MaDangPhim = dto.MaDangPhim != 0 ? dto.MaDangPhim : phim.MaDangPhim;
                phim.DoTuoi = dto.DoTuoi != 0 ? dto.DoTuoi : phim.DoTuoi;

                await _db.SaveChangesAsync();
                Console.WriteLine("Cập nhật phim thành công.");

                return Ok(new { Message = "Cập nhật phim thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật phim: {ex.Message}");
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }

        // Xóa phim
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhim(int id)
        {
            try
            {
                Console.WriteLine($"Bắt đầu xóa phim với ID: {id}");
                var film = await _db.Phims.FindAsync(id);
                if (film == null)
                {
                    Console.WriteLine("Không tìm thấy phim để xóa.");
                    return BadRequest(new { Message = "Không tìm thấy phim!" });
                }

                _db.Remove(film);
                await _db.SaveChangesAsync();
                Console.WriteLine("Xóa phim thành công.");

                return Ok(new { Message = "Xóa phim thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa phim: {ex.Message}");
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }
}
