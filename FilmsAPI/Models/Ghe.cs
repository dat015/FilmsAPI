using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Ghe
{
    public int SoGhe { get; set; }

    public int MaPhong { get; set; }

    public bool TrangThaiGhe { get; set; }

    public int MaTinhTrang { get; set; }

    public int MaLoaiGhe { get; set; }

    public virtual LoaiGhe MaLoaiGheNavigation { get; set; } = null!;

    public virtual PhongChieu MaPhongNavigation { get; set; } = null!;

    public virtual TinhTrang MaTinhTrangNavigation { get; set; } = null!;

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
