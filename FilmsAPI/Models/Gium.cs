using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Gium
{
    public int MaGia { get; set; }

    public decimal SoTien { get; set; }

    public int MaDangPhim { get; set; }

    public int MaVe { get; set; }

    public int IdLoaiGhe { get; set; }

    public virtual LoaiGhe IdLoaiGheNavigation { get; set; } = null!;

    public virtual DangPhim MaDangPhimNavigation { get; set; } = null!;
}
