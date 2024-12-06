namespace FilmsAPI.DTO
{
    public class RegisterDTO
    {
        public string Sdt { get; set; } // Số điện thoại
        public string Password { get; set; } // Mật khẩu
        public string ConfirmPassword { get; set; } // Xác nhận mật khẩu
        public string TenNhanVien { get; set; } // Tên nhân viên
        public string Email { get; set; } // Email
        public int MaQuyen { get; set; } // Vai trò (Admin hoặc Nhân viên)
    }
}
