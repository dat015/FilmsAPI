using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.DTO;
using FilmsAPI.Filters;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

    public class GheController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public GheController()
        {
            _db = new FilmsDbContext();
        }

        // GET: api/Ghe/{maPhong}
        [HttpGet("{maPhong}", Name = "GetDanhSachGhe")]
        public async Task<IActionResult> GetDanhSachGhe(int maPhong)
        {
            try
            {
                // Lấy danh sách ghế từ phòng chiếu và chỉ lấy những thuộc tính cần thiết
                var ds = await _db.Ghes
                    .Where(d => d.MaPhong == maPhong)
                    .Select(g => new
                    {
                        g.MaGhe,              // Mã ghế
                        g.MaLoaiGhe,          // Mã loại ghế    
                        g.MaPhong,            // Mã phòng chiếu
                        TenLoaiGhe = g.MaLoaiGheNavigation.TenLoaiGhe, // Tên loại ghế
                        SoGheMotHang = g.MaPhongNavigation.SoGheMotHang
                    })
                    .ToListAsync();

                // Kiểm tra nếu không có ghế nào trong phòng chiếu
                if (ds == null || !ds.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy ghế nào cho mã phòng này." });
                }

                // Trả về danh sách ghế nếu có dữ liệu
                return Ok(ds);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi có vấn đề trong quá trình truy vấn
                return BadRequest(new { Message = "Lỗi khi truy vấn cơ sở dữ liệu", Error = ex.Message });
            }
        }


        // PUT: api/Ghe
        [HttpPut(Name = "UpdateGhe")]
        public async Task<IActionResult> UpdateGhe([FromBody] Ghe dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                // Tìm ghế theo MaGhe (để cập nhật)
                var ghe = await _db.Ghes.FindAsync(dto.MaGhe);
                if (ghe == null)
                {
                    return NotFound(new { Message = "Không tìm thấy ghế với ID được cung cấp." });
                }

                ghe.MaPhong = dto.MaPhong;
                ghe.TrangThai = dto.TrangThai;  // Cập nhật trạng thái ghế
                ghe.MaLoaiGhe = dto.MaLoaiGhe;

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Cập nhật ghế thành công!", UpdatedGhe = ghe });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật ghế.", Error = ex.Message });
            }
        }

        // POST: api/Ghe
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

                return Ok(new { Message = "Thêm thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi thêm ghế." + ex.Message });
            }
        }
    }
}
