using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var phim = _db.Phims
                    .Include(p => p.MaDangPhimNavigation) // Lấy thông tin dạng phim
                    .Include(p => p.TheLoaiCuaPhims)
                    .Include(p => p.XuatChieus)           // Lấy thông tin xuất chiếu
                    .ToList();

                return Ok(phim);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // Action to get phim by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var phim = await _db.Phims
                    .Include(p => p.MaDangPhimNavigation) // Lấy thông tin dạng phim
                    .Include(p => p.TheLoaiCuaPhims)
                    .Include(p => p.XuatChieus)           // Lấy thông tin xuất chiếu
                    .FirstOrDefaultAsync(p => p.MaPhim == id); // Ensure to specify the filter by id

                if (phim == null)
                    return NotFound();

                return Ok(phim);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // Thêm phim mới
        [HttpPost(Name = "AddPhim")]
        public async Task<IActionResult> AddPhim([FromBody] Phim dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
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

                return Ok(new { Message = "Thêm phim thành công", phim = phim });
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // Cập nhật phim
        [HttpPut(Name = "UpdatePhim")]
        public async Task<IActionResult> UpdatePhim([FromBody] Phim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Cung cấp đủ dữ liệu!" });
            }

            try
            {
                var phim = await _db.Phims.FindAsync(dto.MaPhim);

                if (phim == null)
                {
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

                return Ok(new { Message = "Cập nhật phim thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhim(int id)
        {
            try
            {
                var film = await _db.Phims.FindAsync(id);
                if(film == null)
                {
                    return BadRequest();
                }

                _db.Remove(film);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
