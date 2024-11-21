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
<<<<<<< HEAD
        public async Task<IActionResult> GetNhanVien()
        { 
            var nhanvien =await _db.NhanViens.ToListAsync();
            return Ok(nhanvien);
=======
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
>>>>>>> 8c6313c3468e6612e8e53f2a8df1383eb68b3410
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
        [HttpPut(Name = "UpdateNhanVien")]
        public async Task<IActionResult> UpdateNhanVien([FromBody] NhanVien dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                // Cập nhật thông tin nhân viên
                var nhanVien = await _db.NhanViens.FindAsync(dto.MaNv);
                if (nhanVien == null)
                {
                    return NotFound("Không tìm thấy nhân viên");
                }

                nhanVien.TenNv = dto.TenNv ?? nhanVien.TenNv;
                nhanVien.Sdt = dto.Sdt ?? nhanVien.Sdt;
                nhanVien.Email = dto.Email ?? nhanVien.Email;
                nhanVien.MatKhau = dto.MatKhau ?? nhanVien.MatKhau;
                nhanVien.MaQuyen = dto.MaQuyen != 0 ? dto.MaQuyen : nhanVien.MaQuyen;

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
