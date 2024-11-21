using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Phim
{
    public int MaPhim { get; set; }

    public string TenPhim { get; set; } = null!;

    public string GhiChu { get; set; } = null!;

    public string Mota { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public int ThoiLuong { get; set; }

    public DateOnly NgayKc { get; set; }

    public DateOnly NgayKt { get; set; }

    public int DoTuoi { get; set; }

    public string AnhBia { get; set; } = null!;

    public string TenDaoDien { get; set; } = null!;

    public int MaDangPhim { get; set; }

    public virtual DangPhim? MaDangPhimNavigation { get; set; } = null!;

    public virtual ICollection<TheLoaiCuaPhim>? TheLoaiCuaPhims { get; set; } = new List<TheLoaiCuaPhim>();

    public virtual ICollection<XuatChieu>? XuatChieus { get; set; } = new List<XuatChieu>();
}
