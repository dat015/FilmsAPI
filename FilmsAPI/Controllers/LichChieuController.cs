using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichChieuController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public LichChieuController(FilmsDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetXuatChieu")]
        public async Task<IActionResult> GetXuatChieu()
        {
            try
            {
                var xuatChieu = await _db.XuatChieus
                    .Include(x => x.MaPhimNavigation)
                    .ThenInclude(x => x.TheLoaiCuaPhims)
                    .ThenInclude(x => x.MaTheLoaiNavigation)
                    .Include(x => x.MaPhongNavigation)
                    .ToListAsync();

                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        } 
        [HttpGet("GetXuatChieuByTime")]
        public async Task<IActionResult> GetXuatChieuByTime()
        {
            try
            {
                var xuatChieu = await _db.XuatChieus
                    .Include(x => x.MaPhimNavigation)
                    .ThenInclude(x => x.TheLoaiCuaPhims)
                    .ThenInclude(x => x.MaTheLoaiNavigation)
                    .Include(x => x.MaPhongNavigation)
                    .Where(item => item.ThoiGianBatDau > DateTime.Now)
                    .ToListAsync();

                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Lấy tất cả các phim
        [HttpGet("GetAllPhim")]
        public async Task<IActionResult> GetAllPhim()
        {
            try
            {
                var allPhim = await _db.Phims
                   .Include(p => p.XuatChieus)
                   .Include(p => p.TheLoaiCuaPhims)
                   .ThenInclude(tlc => tlc.MaTheLoaiNavigation) // Đảm bảo tải MaTheLoaiNavigation
                   .Select(p => new
                   {
                       p.MaPhim,
                       p.TenPhim,
                       p.AnhBia,
                       p.ThoiLuong,
                       p.NgayKc,
                       TheLoai = p.TheLoaiCuaPhims.Select(tlc => new
                       {
                           tlc.MaTheLoai,
                           TenTheLoai = tlc.MaTheLoaiNavigation != null
                                        ? tlc.MaTheLoaiNavigation.TenTheLoai
                                        : "Chưa xác định"
                       })
                   })
                   .ToListAsync();

                return Ok(allPhim);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // Lấy tất cả các thể loại
        [HttpGet("GetAllTheLoai")]
        public async Task<IActionResult> GetAllTheLoai()
        {
            try
            {
                var allTheLoai = await _db.LoaiPhims
                    .Select(tl => new
                    {
                        tl.MaTheLoai,
                        tl.TenTheLoai
                    })
                    .ToListAsync();

                return Ok(allTheLoai);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
