using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class ManHinh
{
    public int MaManHinh { get; set; }

    public string TenManHinh { get; set; } = null!;

    public virtual ICollection<DangPhim>? DangPhims { get; set; } = new List<DangPhim>();

    public virtual ICollection<PhongChieu>? PhongChieus { get; set; } = new List<PhongChieu>();
}
