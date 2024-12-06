using FilmsAPI.Models;

namespace FilmsAPI.DTO
{
    public class LoginDTO
    {

        public required string Sdt { get; set; }
        public required string Password { get; set; }
        public NhanVien? User { get; set; }

    }
}
