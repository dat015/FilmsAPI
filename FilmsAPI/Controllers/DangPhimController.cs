using FilmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
=======
using System.Threading.Tasks;
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410

namespace FilmsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DangPhimController : ControllerBase
    {
<<<<<<< HEAD
        private readonly FilmsmanageDbContext _db;

        public DangPhimController(FilmsmanageDbContext db)
        {
            _db = db;
        }

=======
        private readonly FilmsDbContext _db;

        // Dependency Injection cho DbContext
        public DangPhimController()
        {
            _db = new FilmsDbContext();
        }

        // GET: /DangPhim
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
        [HttpGet(Name = "GetDangPhim")]
        public async Task<IActionResult> GetDangPhim()
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
                var dangPhims = await _db.DangPhims.ToListAsync();
                if (dangPhims == null || dangPhims.Count == 0)
                {
                    return NotFound(new { message = "Không có dữ liệu dạng phim nào." });
                }
=======
=======
                //day la cmt
>>>>>>> 929d576b2d3e51fdab03da8214fa51ca1cd8d022
                var dangPhims = await _db.DangPhims
                    .Include(d => d.Phims)
                    .Include(d => d.MaManHinhNavigation)
                    .ToListAsync();

                if (dangPhims == null || !dangPhims.Any())
                {
                    return NotFound(new { message = "Không có dữ liệu dạng phim." });
                }

>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
                return Ok(dangPhims);
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // Cập nhật dạng phim
=======
                return BadRequest(new { message = "Lỗi khi truy vấn cơ sở dữ liệu.", error = ex.Message });
            }
        }

        // PUT: /DangPhim
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
        [HttpPut(Name = "UpdateDangPhim")]
        public async Task<IActionResult> Update([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống." });
            }

            try
            {
                var dangPhim = await _db.DangPhims
                    .FirstOrDefaultAsync(dp => dp.MaDangPhim == dto.MaDangPhim);

                if (dangPhim == null)
                {
                    return NotFound(new { message = "Không tìm thấy bản ghi cần cập nhật." });
                }

                // Cập nhật các trường dữ liệu
                dangPhim.TenDangPhim = dto.TenDangPhim;
                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi cập nhật: " + ex.Message });
            }
        }

        // Thêm mới dạng phim
=======
                return BadRequest(new { message = "Đã xảy ra lỗi khi cập nhật.", error = ex.Message });
            }
        }

        // POST: /DangPhim
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
        [HttpPost(Name = "CreateDangPhim")]
        public async Task<IActionResult> Create([FromBody] DangPhim dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            if (string.IsNullOrWhiteSpace(dto.TenDangPhim))
            {
                return BadRequest(new { message = "Tên dạng phim không được để trống." });
            }

            try
            {
                var existingDangPhim = await _db.DangPhims
                    .Where(d => d.TenDangPhim.ToLower() == dto.TenDangPhim.ToLower())
                    .FirstOrDefaultAsync();

                if (existingDangPhim != null)
                {
                    return BadRequest(new { message = "Dạng phim này đã tồn tại." });
                }

                var dangPhim = new DangPhim
                {
                    TenDangPhim = dto.TenDangPhim,
                    MaManHinh = dto.MaManHinh // Thiết lập ManHinh cho DangPhim
                };

                _db.DangPhims.Add(dangPhim);
                await _db.SaveChangesAsync();

<<<<<<< HEAD
                return CreatedAtAction(nameof(GetDangPhim), new { id = dangPhim.MaDangPhim }, new { message = "Thêm mới thành công", dangPhimId = dangPhim.MaDangPhim });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi tạo mới: " + ex.Message });
=======
                return CreatedAtAction("GetDangPhim", new { id = dangPhim.MaDangPhim }, dangPhim);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi thêm mới dạng phim.", error = ex.Message });
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
            }
        }
    }
}
