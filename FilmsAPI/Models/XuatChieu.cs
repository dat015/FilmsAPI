using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class XuatChieu
{
    public int MaXuatChieu { get; set; }

    public int MaPhim { get; set; }

    public int GioChieu { get; set; }

    public int PhutChieu { get; set; }

    public int IdPhongChieu { get; set; }

    public virtual ICollection<DanhSachDatVeOnline> DanhSachDatVeOnlines { get; set; } = new List<DanhSachDatVeOnline>();

    public virtual PhongChieu IdPhongChieuNavigation { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();

    public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
}
