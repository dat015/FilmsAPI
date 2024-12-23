﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilmsAPI.Filters;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

    public class LoaiPhimController : ControllerBase
    {
        private readonly FilmsDbContext _context;

        public LoaiPhimController()
        {
            _context = new FilmsDbContext();
        }

        // GET: api/LoaiPhim
        [HttpGet]
        public async Task<IActionResult> GetAllLoaiPhim()
        {
            try
            {
                var loaiPhims = await _context.LoaiPhims
                                    .Include(p => p.TheLoaiCuaPhims)
                                    .ToListAsync();

                if (loaiPhims == null || loaiPhims.Count == 0)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim nào." });
                }

                return Ok(loaiPhims);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/LoaiPhim/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoaiPhimById(int id)
        {
            try
            {
                var loaiPhim = await _context.LoaiPhims
                    .FirstOrDefaultAsync(lp => lp.MaTheLoai == id);

                if (loaiPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim với MaTheLoai " + id });
                }

                return Ok(loaiPhim);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/LoaiPhim
        [HttpPost]
        public async Task<IActionResult> CreateLoaiPhim([FromBody] LoaiPhim loaiPhim)
        {
            if (loaiPhim == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                _context.LoaiPhims.Add(loaiPhim);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Thêm thể loại thành công"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/LoaiPhim/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoaiPhim([FromBody] LoaiPhim loaiPhim)
        {
            if (loaiPhim == null || loaiPhim.MaTheLoai != loaiPhim.MaTheLoai)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ hoặc MaTheLoai không khớp." });
            }

            try
            {
                var existingLoaiPhim = await _context.LoaiPhims
                    .FirstOrDefaultAsync(lp => lp.MaTheLoai == loaiPhim.MaTheLoai);

                if (existingLoaiPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy loại phim với MaTheLoai " + loaiPhim.MaTheLoai });
                }

                // Cập nhật thông tin loại phim
                existingLoaiPhim.TenTheLoai = loaiPhim.TenTheLoai;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật loại phim thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/LoaiPhim/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiPhim(int id)
        {
            try
            {
                var loaiPhim = await _context.LoaiPhims
                    .FirstOrDefaultAsync(lp => lp.MaTheLoai == id);

                if (loaiPhim == null)
                {
                    return NotFound();
                }

                _context.LoaiPhims.Remove(loaiPhim);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
