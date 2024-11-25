using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly FilmsDbContext _db;

        public NhanVienController()
        {
            _db = new FilmsDbContext();
        }

        // GET: api/NhanVien
        [HttpGet(Name = "GetNhanVien")]
        public IActionResult Get()
        {
            try
            {
                var nhanvien = _db.NhanViens
                    .Include(nv => nv.MaQuyenNavigation)  // Bao gồm thông tin quyền của nhân viên
                    .ToList();

                return Ok(nhanvien);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        //Lấy nhân viên theo mã nhân viên
        [HttpGet("{manhanvien}", Name = "GetNhanVienById")]
        public async Task<IActionResult> GetNhanVienById(int manhanvien)
        {
            var nhanVien = await _db.NhanViens.FindAsync(manhanvien);
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy nhân viên");
            }
            return Ok(nhanVien);
        }

            // POST: api/NhanVien (Sử dụng POST thay vì PUT cho thêm mới)
            [HttpPost(Name = "AddNhanVien")]
        public async Task<IActionResult> AddNhanVien([FromBody] NhanVien dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                // Thêm nhân viên mới
                var nhanVien = new NhanVien
                {
                    TenNv = dto.TenNv,
                    Sdt = dto.Sdt,
                    Email = dto.Email,
                    MatKhau = dto.MatKhau,
                    MaQuyen = dto.MaQuyen
                };

                await _db.NhanViens.AddAsync(nhanVien);
                await _db.SaveChangesAsync();
                return Ok("Thêm nhân viên thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        // PUT: api/NhanVien (Cập nhật thông tin nhân viên)
        [HttpPut("{manhanvien}", Name = "UpdateNhanVien")]
        public async Task<IActionResult> UpdateNhanVien(int manhanvien, [FromBody] NhanVien dto)
        {
            var nhanVien = await _db.NhanViens.FindAsync(manhanvien);
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy nhân viên");
            }
            nhanVien.TenNv = dto.TenNv;
            nhanVien.Sdt = dto.Sdt;
            nhanVien.Email = dto.Email;
            nhanVien.MatKhau = dto.MatKhau;
            nhanVien.MaQuyen = dto.MaQuyen;
            try
            {
                await _db.SaveChangesAsync();
                return Ok("Cập nhật nhân viên thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{manhanvien}", Name = "DeleteNhanVien")]
        public async Task<IActionResult> DeleteNhanVien(int manhanvien)
        {
            try
            {
                var nhanVien = await _db.NhanViens.FindAsync(manhanvien);
                if (nhanVien == null)
                {
                    return NotFound("Không tìm thấy nhân viên");
                }

                _db.NhanViens.Remove(nhanVien);
                await _db.SaveChangesAsync();
                return Ok("Xóa nhân viên thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }
}
