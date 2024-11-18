using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhimController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;
        public PhimController()
        {
            _db = new FilmsmanageDbContext() ;
        }
        [HttpGet(Name = "GetPhim")]
        public IActionResult Get()
        {
            var phim = _db.Phims.ToList();
            return Ok(phim);
        }
        [HttpPut(Name = "AddPhim")]
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
                    GhiChu = dto.GhiChu,
                    DoDaiGio = dto.DoDaiGio,
                    DoDaiPhut = dto.DoDaiPhut,
                    NgayBatDau = dto.NgayBatDau,
                    MoTaPhim = dto.MoTaPhim,
                    AnhDaiDien = dto.AnhDaiDien,
                    NgayKetThuc = dto.NgayKetThuc,
                    MaLoaiPhim = dto.MaLoaiPhim,
                    MaDangPhim = dto.MaDangPhim,
                    MaXuatChieu = dto.MaXuatChieu,
                    NoiDungPhim = dto.NoiDungPhim,
                    DoTuoi = dto.DoTuoi,
                    IdQuocGia = dto.IdQuocGia
                };
                await _db.Phims.AddAsync(phim);
                await _db.SaveChangesAsync();
                return Ok("Thêm phim thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        [HttpPost(Name = "UpdatePhim")]
        public async Task<IActionResult> UpdatePhim([FromBody] Phim dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }
            try
            {
                var phim = await _db.Phims.FindAsync(dto.MaPhim);
                phim = dto;
                await _db.SaveChangesAsync();
                return Ok("Cập nhật phim thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }
}
