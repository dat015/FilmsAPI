using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public DateTime NgaySinh { get; set; }

    public string DiaChi { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? Email { get; set; }

    public bool GioiTinh { get; set; }

    public string MatKhau { get; set; } = null!;

    public virtual ICollection<DanhSachDatVeOnline> DanhSachDatVeOnlines { get; set; } = new List<DanhSachDatVeOnline>();
}
