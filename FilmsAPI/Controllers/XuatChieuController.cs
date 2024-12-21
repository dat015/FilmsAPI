using FilmsAPI.Filters;
using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilter("Admin")]

    public class XuatChieuController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public XuatChieuController()
        {
            _db = new FilmsDbContext();
        }

        [HttpGet(Name = "GetXuatChieu")]
        public async Task<ActionResult> GetXuatChieu()
        {
            try
            {
                var xuatChieu = await _db.XuatChieus
                    .Include(x => x.MaPhimNavigation)
                    .Include(x => x.MaPhongNavigation)
                    .ToListAsync();
                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetXuatChieu(int id)
        {
            try
            {
                var xuatChieu = await _db.XuatChieus
                 .Include(x => x.MaPhimNavigation)
                 .Include(x => x.MaPhongNavigation)
                 .FirstOrDefaultAsync(x => x.MaXuatChieu == id);

                if (xuatChieu == null)
                {
                    return NotFound(new { message = "Không tìm thấy xuất chiếu." });
                }

                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddXuatChieu([FromBody] XuatChieu dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Cung cấp đủ dữ liệu" });
            }

            try
            {
                // Kiểm tra thời gian bắt đầu phải >= thời gian hiện tại
                if (dto.ThoiGianBatDau <= DateTime.Now)
                {
                    return BadRequest(new { Message = "Thời gian bắt đầu phải lớn hơn hoặc bằng thời gian hiện tại." });
                }

                // Kiểm tra thời gian kết thúc phải lớn hơn thời gian bắt đầu
                if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
                {
                    return BadRequest(new { Message = "Thời gian kết thúc phải lớn hơn thời gian bắt đầu." });
                }

                // Kiểm tra phòng chiếu không trùng với các suất chiếu khác
                var existingShowtime = await _db.XuatChieus
                    .Where(x => x.MaPhong == dto.MaPhong
                                && x.ThoiGianBatDau < dto.ThoiGianKetThuc
                                && x.ThoiGianKetThuc > dto.ThoiGianBatDau)
                    .AnyAsync();
                if (existingShowtime)
                {
                    return BadRequest(new { Message = "Phòng chiếu đã được sử dụng cho một phim khác trong khoảng thời gian này." });
                }

                // Kiểm tra không trùng lặp suất chiếu (cùng phim, cùng phòng, cùng thời gian)
                var duplicateShowtime = await _db.XuatChieus
                    .Where(x => x.MaPhim == dto.MaPhim
                                && x.MaPhong == dto.MaPhong
                                && x.ThoiGianBatDau == dto.ThoiGianBatDau
                                && x.ThoiGianKetThuc == dto.ThoiGianKetThuc)
                    .AnyAsync();
                if (duplicateShowtime)
                {
                    return BadRequest(new { Message = "Suất chiếu bị trùng lặp." });
                }

                // Giới hạn số suất chiếu trong một ngày cho phòng
                var showtimesCount = await _db.XuatChieus
                 .Where(x => x.MaPhong == dto.MaPhong
                             && x.ThoiGianBatDau.HasValue
                             && x.ThoiGianBatDau.Value.Date == dto.ThoiGianBatDau.Value.Date)
                 .CountAsync();

                if (showtimesCount >= 10)
                {
                    return BadRequest(new { Message = "Phòng chiếu đã đạt giới hạn số suất chiếu trong ngày." });
                }

                // Thêm mới suất chiếu
                _db.Add(dto);
                await _db.SaveChangesAsync();
                return Ok(new { Message = "Thêm thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut(Name = "UpdateXuatChieu")]
        public async Task<IActionResult> UpdateXuatChieu([FromBody] XuatChieu dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Cung cấp đủ dữ liệu" });
            }

            try
            {
                var xuatChieu = await _db.XuatChieus.FirstOrDefaultAsync(v => v.MaXuatChieu == dto.MaXuatChieu);

                if (xuatChieu == null)
                {
                    return NotFound(new { Message = "Không tìm thấy bản ghi cần cập nhật" });
                }

                // Kiểm tra thời gian bắt đầu phải >= thời gian hiện tại
                if (dto.ThoiGianBatDau < DateTime.Now)
                {
                    return BadRequest(new { Message = "Thời gian bắt đầu phải lớn hơn hoặc bằng thời gian hiện tại." });
                }

                // Kiểm tra thời gian kết thúc phải lớn hơn thời gian bắt đầu
                if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
                {
                    return BadRequest(new { Message = "Thời gian kết thúc phải lớn hơn thời gian bắt đầu." });
                }

                // Kiểm tra phòng chiếu không trùng với các suất chiếu khác
                var existingShowtime = await _db.XuatChieus
                    .Where(x => x.MaPhong == dto.MaPhong
                                && x.ThoiGianBatDau < dto.ThoiGianKetThuc
                                && x.ThoiGianKetThuc > dto.ThoiGianBatDau
                                && x.MaXuatChieu != dto.MaXuatChieu) // tránh trùng chính nó
                    .AnyAsync();
                if (existingShowtime)
                {
                    return BadRequest(new { Message = "Phòng chiếu đã được sử dụng cho một phim khác trong khoảng thời gian này." });
                }

                // Kiểm tra không trùng lặp suất chiếu (cùng phim, cùng phòng, cùng thời gian)
                var duplicateShowtime = await _db.XuatChieus
                    .Where(x => x.MaPhim == dto.MaPhim
                                && x.MaPhong == dto.MaPhong
                                && x.ThoiGianBatDau == dto.ThoiGianBatDau
                                && x.ThoiGianKetThuc == dto.ThoiGianKetThuc
                                && x.MaXuatChieu != dto.MaXuatChieu) // tránh trùng chính nó
                    .AnyAsync();
                if (duplicateShowtime)
                {
                    return BadRequest(new { Message = "Suất chiếu bị trùng lặp." });
                }

                // Giới hạn số suất chiếu trong một ngày cho phòng
                var showtimesCount = await _db.XuatChieus
                    .Where(x => x.MaPhong == dto.MaPhong
                                && x.ThoiGianBatDau.Value.Date == dto.ThoiGianBatDau.Value.Date)
                    .CountAsync();
                if (showtimesCount >= 10)
                {
                    return BadRequest(new { Message = "Phòng chiếu đã đạt giới hạn số suất chiếu trong ngày." });
                }

                // Cập nhật bản ghi suất chiếu
                xuatChieu.MaPhim = dto.MaPhim;
                xuatChieu.MaPhong = dto.MaPhong;
                xuatChieu.Status = dto.Status;
                xuatChieu.ThoiGianBatDau = dto.ThoiGianBatDau;
                xuatChieu.ThoiGianKetThuc = dto.ThoiGianKetThuc;

                await _db.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetByMovie/{maPhim}")]
        public async Task<IActionResult> GetShowtimeByMovie(int maPhim)
        {
            try
            {
                var xuatChieu = await _db.XuatChieus
                    .Where(x => x.MaPhim == maPhim)
                    .Include(x => x.MaPhimNavigation)
                    .Include(x => x.MaPhongNavigation)
                    .ToListAsync();

                if (xuatChieu == null || !xuatChieu.Any())
                {
                    return NotFound(new { Message = "Không có suất chiếu cho phim này." });
                }

                return Ok(xuatChieu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("Delete/{maXuatChieu}")]
        public async Task<IActionResult> DeleteShowtime(int maXuatChieu)
        {
            try
            {
                var xuatChieu = await _db.XuatChieus.FirstOrDefaultAsync(x => x.MaXuatChieu == maXuatChieu);
                if (xuatChieu == null)
                {
                    return NotFound("Không tìm thấy suất chiếu cần xóa.");
                }

                _db.XuatChieus.Remove(xuatChieu);
                await _db.SaveChangesAsync();

                return Ok("Xóa suất chiếu thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

