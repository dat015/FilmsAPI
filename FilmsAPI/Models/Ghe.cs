using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Ghe
{
    public int MaGhe { get; set; }

    public bool TrangThai { get; set; }

    public int MaPhong { get; set; }

    public int MaLoaiGhe { get; set; }

    public string TenGhe { get; set; } = null!;

    public virtual LoaiGhe? MaLoaiGheNavigation { get; set; } = null!;

    public virtual PhongChieu? MaPhongNavigation { get; set; } = null!;

    public virtual ICollection<Ve>? Ves { get; set; } = new List<Ve>();
}
