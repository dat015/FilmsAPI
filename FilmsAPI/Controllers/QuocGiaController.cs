using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuocGiaController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        public QuocGiaController()
        {
            _db = new FilmsmanageDbContext();
        }

        [HttpGet(Name = "GetQuocGia")]
        public IActionResult Get()
        {
            var quocGia = _db.QuocGia.ToList();
            return Ok(quocGia);
        }

        [HttpPut(Name = "AddQuocGia")]
        public async Task<IActionResult> AddQuocGia([FromBody] QuocGia dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var quocGia = new QuocGia
                {
                    TenNuoc = dto.TenNuoc,
                };

                await _db.QuocGia.AddAsync(quocGia);
                await _db.SaveChangesAsync();
                return Ok("Thêm quốc gia thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        [HttpPost(Name = "UpdateQuocGia")]
        public async Task<IActionResult> UpdateQuocGia([FromBody] QuocGia dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                var quocGia = await _db.QuocGia.FindAsync(dto.IdQuocGia);
                if (quocGia == null)
                {
                    return NotFound("Không tìm thấy quốc gia");
                }

                quocGia.TenNuoc = dto.TenNuoc;

                await _db.SaveChangesAsync();
                return Ok("Cập nhật quốc gia thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

       
    }
}
