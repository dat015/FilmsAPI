using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Đảm bảo bạn đã thêm namespace này
using FilmsAPI.Models;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly FilmsmanageDbContext _db;

        // Sử dụng Dependency Injection để khởi tạo DbContext
        public NhanVienController()
        {
            _db = new FilmsmanageDbContext();
        }

        // GET: api/NhanVien
        [HttpGet(Name = "GetNhanVien")]
        public IActionResult Get()
        { 
            var nhanvien = _db.NhanViens.ToList();
            return Ok(nhanvien);
        }

        // POST: api/NhanVien (Sử dụng POST thay vì PUT cho thêm mới)
        [HttpPut(Name = "AddNhanVien")]
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
                    TenDangNhap = dto.TenDangNhap,
                    MatKhau = dto.MatKhau,
                    TenNhanVien = dto.TenNhanVien,
                    GioiTinh = dto.GioiTinh,
                    NgaySinh = dto.NgaySinh,
  
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
        [HttpPost(Name = "UpdateNhanVien")]
        public async Task<IActionResult> UpdateNhanVien([FromBody] NhanVien dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }
            try
            {
                // Cập nhật thông tin nhân viên
                var nhanVien = await _db.NhanViens.FindAsync(dto.IdNhanVien);
                if (nhanVien == null)
                {
                    return NotFound("Không tìm thấy nhân viên");
                }

                nhanVien.TenDangNhap = dto.TenDangNhap;
                nhanVien.MatKhau = dto.MatKhau;
                nhanVien.TenNhanVien = dto.TenNhanVien;
                nhanVien.GioiTinh = dto.GioiTinh;
                nhanVien.NgaySinh = dto.NgaySinh;
                nhanVien.MaQuyen = dto.MaQuyen;

                await _db.SaveChangesAsync();
                return Ok("Cập nhật nhân viên thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }
}
