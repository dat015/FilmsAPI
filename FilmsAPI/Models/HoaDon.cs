using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public decimal TongTien { get; set; }

    public int MaNv { get; set; }
    public int? MaKh { get; set; } = null;
    public virtual ICollection<ChiTietHoaDon>? ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual NhanVien? MaNvNavigation { get; set; } = null!;

}
