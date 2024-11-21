using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public int MaKh { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public virtual KhachHang? MaKhNavigation { get; set; } = null!;
}
