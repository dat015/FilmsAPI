using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class LoaiGhe
{
    public int MaLoai { get; set; }

    public string TenLoaiGhe { get; set; } = null!;

    public double TrongSo { get; set; } = 1;

    public virtual ICollection<Ghe>? Ghes { get; set; } = new List<Ghe>();
}
