using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class XuatChieu
{
    public int MaXuatChieu { get; set; }

    public DateTime? ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }
    public decimal? GiaCoBan { get; set; }

    public int MaPhong { get; set; }

    public int MaPhim { get; set; }

    public bool Status { get; set; }

    public virtual Phim? MaPhimNavigation { get; set; } = null!;

    public virtual PhongChieu? MaPhongNavigation { get; set; } = null!;

    public virtual ICollection<Ve>? Ves { get; set; } = new List<Ve>();
}
