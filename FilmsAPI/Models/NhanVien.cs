using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public string TenNv { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public int MaQuyen { get; set; }

    public virtual ICollection<HoaDon>? HoaDons { get; set; } = new List<HoaDon>();

    public virtual Quyen? MaQuyenNavigation { get; set; } = null!;

    public virtual ICollection<Ve>? Ves { get; set; } = new List<Ve>();
}
