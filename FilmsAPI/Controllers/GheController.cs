using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.DTO;

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
        public async Task<IActionResult> GetDanhSachGhe(int maPhong)
        {
            try
            {
                var ds = await _db.Ghes
                .Where(d => d.MaPhong == maPhong)
                    .ToListAsync();

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

        [HttpPut(Name = "UpdateGhe")]
        public async Task<IActionResult> UpdateGhe([FromBody] Ghe dto)
        {
            // Kiểm tra nếu dto là null
            if (dto == null)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                // Tìm ghế theo soGhe từ dto
                var ghe = await _db.Ghes.FindAsync(dto.SoGhe);

                // Kiểm tra nếu ghế không tồn tại
                if (ghe == null)
                {
                    return NotFound(new { Message = "Không tìm thấy ghế với ID được cung cấp." });
                }

                // Cập nhật các giá trị của ghế
                ghe.MaPhong = dto.MaPhong;
                ghe.TrangThaiGhe = dto.TrangThaiGhe;
                ghe.MaTinhTrang = dto.MaTinhTrang;
                ghe.MaLoaiGhe = dto.MaLoaiGhe;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _db.SaveChangesAsync();

                // Trả về thông tin đã cập nhật
                return Ok(new { Message = "Cập nhật ghế thành công!", UpdatedGhe = GetDanhSachGhe(ghe.MaPhong) });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật ghế.", Error = ex.Message });
            }
        }


        [HttpPost(Name = "AddGhe")]
        public async Task<IActionResult> AddGhe([FromBody] Ghe dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ!" });
            }
            try
            {

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
