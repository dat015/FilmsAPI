using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class XuatChieu
{
    public int MaXuatChieu { get; set; }

    public DateOnly ThoiGianBatDau { get; set; }

    public DateOnly ThoiGianKetThuc { get; set; }

    public int MaPhong { get; set; }

    public int MaPhim { get; set; }

    public virtual Phim? MaPhimNavigation { get; set; } = null!;

    public virtual PhongChieu? MaPhongNavigation { get; set; } = null!;

    public virtual ICollection<Ve>? Ves { get; set; } = new List<Ve>();
}
