using FilmsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinhTrangController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public TinhTrangController()
        {
            _db = new FilmsmanageDbContext();
        }

        [HttpGet(Name = "GetTinhTrang")]
        public async Task<IActionResult> GetTinhTrang()
        {
            try
            {
                var tinhTrang = await _db.TinhTrangs.ToListAsync();
                return Ok(tinhTrang);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPut(Name = "AddTinhTrang")]
        public async Task<IActionResult> AddTinhTrang([FromBody] TinhTrang dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var tinhTrang = new TinhTrang
                {
                    TenTinhTrang = dto.TenTinhTrang
                };

                _db.TinhTrangs.Add(tinhTrang);
                await _db.SaveChangesAsync();
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "UpdateTinhTrang")]
        public async Task<IActionResult> UpdateTinhTrang([FromBody] TinhTrang dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenTinhTrang))
            {
                return BadRequest("Tên tình trạng không được để trống");
            }

            try
            {
                var tinhTrang = await _db.TinhTrangs.FirstOrDefaultAsync(dp => dp.MaTinhTrang == dto.MaTinhTrang);

                if (tinhTrang == null)
                {
                    return NotFound("Không tìm thấy bản ghi cần cập nhật");
                }

                tinhTrang.TenTinhTrang = dto.TenTinhTrang;

                await _db.SaveChangesAsync();
                return Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
