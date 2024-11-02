using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class PhongChieu
{
    public int IdPhongChieu { get; set; }

    public string TenPhongChieu { get; set; } = null!;

    public virtual ICollection<Ghe> Ghes { get; set; } = new List<Ghe>();

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();

    public virtual ICollection<XuatChieu> XuatChieus { get; set; } = new List<XuatChieu>();
}
