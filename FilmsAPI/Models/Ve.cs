using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Ve
{
    public int IdVe { get; set; }

    public int MaPhong { get; set; }

    public int MaGia { get; set; }

    public decimal DonGia { get; set; }

    public int MaXuatChieu { get; set; }

    public int IdNhanVien { get; set; }

    public int SoGhe { get; set; }

    public int MaLichChieu { get; set; }

    public virtual NhanVien IdNhanVienNavigation { get; set; } = null!;

    public virtual PhongChieu MaPhongNavigation { get; set; } = null!;

    public virtual XuatChieu MaXuatChieuNavigation { get; set; } = null!;

    public virtual Ghe SoGheNavigation { get; set; } = null!;
}
