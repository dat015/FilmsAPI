using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class PhongChieu
{
    public int MaPhongChieu { get; set; }

    public int SoGhe { get; set; }

    public int SoGheMotHang { get; set; }

    public int? SoHangGhe { get; set; }

    public int MaManHinh { get; set; }

    public string TenPhongChieu { get; set; } = null!;

    public virtual ICollection<Ghe>? Ghes { get; set; } = new List<Ghe>();

    public virtual ManHinh? MaManHinhNavigation { get; set; } = null!;

    public virtual ICollection<XuatChieu>? XuatChieus { get; set; } = new List<XuatChieu>();
}
