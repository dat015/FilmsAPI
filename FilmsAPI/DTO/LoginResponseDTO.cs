using FilmsAPI.Models;

namespace FilmsAPI.DTO
{
    public class LoginResponseDTO
    {
        public string? Token { get; set; }
        public NhanVien? User { get; set; }
    }
}
