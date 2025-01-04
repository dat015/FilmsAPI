using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsAPI.Models;
using FilmsAPI.Filters;
using FilmsAPI.Services.Mail;
using FilmsManage.Helper;

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
        [HttpGet("DoiMatKhau/{id}")]
        public async Task<IActionResult> SendEmail(int id)
        {
           

            // Tìm người dùng theo email
            var user = await _db.NhanViens.FirstOrDefaultAsync(u => u.MaNv == id);
            if (user != null)
            {
                string randomKey = GenerateRandomString(6);
                var pass = randomKey.ToMd5Hash(user.RandomKey);
                user.MatKhau = pass;
                await _db.SaveChangesAsync();

                // Gửi lại email xác nhận
                SendMail.SendEmail(user.Email, "Mật khẩu mới của bạn", randomKey, "");
                return Ok("Thay đổi thành công!");

            }


            // Trả về view đăng nhập với thông báo
            return BadRequest("Thay đổi mật khẩu mới thất bại!");
        }
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }
        //Lấy nhân viên theo sdt nhân viên
        [HttpGet("byPhone/{phoneNumber}", Name = "GetNhanVienBySDT")]

        public async Task<IActionResult> GetNhanVienBySDT(string phoneNumber)
        {
            var nhanVien = await _db.NhanViens.FirstOrDefaultAsync(nv => nv.Sdt == phoneNumber);
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy nhân viên");
            }
            return Ok(nhanVien);
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

        [HttpPost(Name = "AddNhanVien")]
        public async Task<IActionResult> AddNhanVien([FromBody] NhanVien dto)
        {
            if (dto == null)
            {
                return BadRequest("Cung cấp đủ dữ liệu");
            }

            try
            {
                string randomKey = GenerateRandomString(6);

                // Thêm nhân viên mới
                var nhanVien = new NhanVien
                {
                    TenNv = dto.TenNv,
                    Sdt = dto.Sdt,
                    Email = dto.Email,
                    MatKhau = randomKey,
                    MaQuyen = dto.MaQuyen,
                    RandomKey = dto.RandomKey,
                    TenAlias = dto.TenAlias,
                };

                await _db.NhanViens.AddAsync(nhanVien);
                await _db.SaveChangesAsync();
                return Ok(randomKey);
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
