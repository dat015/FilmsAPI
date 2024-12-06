using FilmsAPI.DTO;
using FilmsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FilmsAPI.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly FilmsDbContext _db;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new FilmsDbContext();
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO model)
        {
            var user = await _db.NhanViens
                .Where(nv => nv.Sdt == model.Sdt && nv.MatKhau == model.Password)
                .Include(nv => nv.MaQuyenNavigation)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null; // Trả về null nếu thông tin không hợp lệ
            }

            // Tùy chỉnh vai trò (role) dựa vào dữ liệu thực tế trong cơ sở dữ liệu
            var role = user.MaQuyenNavigation?.TenQuyen;
            var token = GenerateJwtToken(user.Sdt, role);

            // Trả về đối tượng chứa token và thông tin nhân viên
            return new LoginResponseDTO
            {
                Token = token,
                User = user
            };
        }

        public async Task<LoginResponseDTO> RegisterAsync(RegisterDTO model)
        {
            // Kiểm tra xem số điện thoại đã được sử dụng chưa
            var existingUser = await _db.NhanViens
                .Where(nv => nv.Sdt == model.Sdt)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return null; // Trả về null nếu số điện thoại đã tồn tại
            }

            // Kiểm tra mật khẩu và xác nhận mật khẩu
            if (model.Password != model.ConfirmPassword)
            {
                return null; // Trả về null nếu mật khẩu không khớp
            }

            // Tạo mới người dùng
            var newUser = new NhanVien
            {
                Sdt = model.Sdt,
                MatKhau = model.Password, // Mật khẩu cần được mã hóa trước khi lưu trữ trong cơ sở dữ liệu
                TenNv = model.TenNhanVien,
                Email = model.Email,
                MaQuyen = model.MaQuyen // Vai trò người dùng
            };

            // Lưu người dùng mới vào cơ sở dữ liệu
            _db.NhanViens.Add(newUser);
            await _db.SaveChangesAsync();

            // Tạo token cho người dùng mới
            var token = GenerateJwtToken(newUser.Sdt, newUser.MaQuyenNavigation?.TenQuyen);

            // Trả về đối tượng LoginResponseDTO chứa token và thông tin người dùng
            return new LoginResponseDTO
            {
                Token = token,
                User = newUser
            };
        }


        public string GenerateJwtToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
