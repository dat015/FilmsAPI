using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public decimal TongTien { get; set; }

    public int MaNv { get; set; }

    public int? MaKh { get; set; }

    public virtual ICollection<ChiTietHoaDon>? ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<DetailFood>? DetailFoods { get; set; } = new List<DetailFood>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; } = null!;
}
