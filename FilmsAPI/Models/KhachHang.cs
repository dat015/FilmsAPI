using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class KhachHang
{
    public int MaKh { get; set; }

    public string? TenKh { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public int? DiemTichLuy { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string Cccd { get; set; } = null!;

    public virtual ICollection<HoaDon>? HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<TaiKhoan>? TaiKhoans { get; set; } = new List<TaiKhoan>();
}
