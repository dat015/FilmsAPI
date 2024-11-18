using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GheController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;
        public GheController()
        {
            _db = new FilmsmanageDbContext();
        }

        [HttpGet("{maPhong}", Name = "GetDanhSachGhe")]
        public  IActionResult GetDanhSachGhe(int maPhong)
        {
            try
            {
                var ds =  _db.Ghes
                    .Where(d => d.MaPhong == maPhong)
                    .ToList();

                if (ds == null || !ds.Any())
                {
                    return NotFound("Không tìm thấy ghế nào cho mã phòng này.");
                }

                return Ok(ds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi truy vấn cơ sở dữ liệu", Error = ex.Message });
            }

        }

        [HttpPut("{id}", Name = "UpdateGhe")]
        public async Task<IActionResult> UpdateGhe(int id, [FromBody] Ghe dto)
        {
            try
            {
                var ghe = await _db.Ghes.FindAsync(id);
                if (ghe == null)
                {
                    return NotFound("Không tìm thấy ghế với ID được cung cấp.");
                }

                ghe.SoGhe = dto.SoGhe;
                ghe.MaPhong = dto.MaPhong;
                ghe.TrangThaiGhe = dto.TrangThaiGhe;
                ghe.MaTinhTrang = dto.MaTinhTrang;
                ghe.MaLoaiGhe = dto.MaLoaiGhe;

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Cập nhật ghế thành công!", UpdatedGhe =  GetDanhSachGhe(ghe.MaPhong) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật ghế.", Error = ex.Message });
            }
        }

        [HttpPost(Name = "AddGhe")]
        public async Task<IActionResult> AddGhe([FromBody] Ghe dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { Message = "Dữ liệu không hợp lệ!" });
                }

                _db.Ghes.Add(dto);

                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetDanhSachGhe", new { maPhong = dto.MaPhong }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi thêm ghế.", Error = ex.Message });
            }
        }


    }
}
