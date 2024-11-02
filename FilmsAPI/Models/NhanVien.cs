using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class NhanVien
{
    public int IdNhanVien { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string TenNhanVien { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public DateTime NgaySinh { get; set; }

    public int MaQuyen { get; set; }

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
