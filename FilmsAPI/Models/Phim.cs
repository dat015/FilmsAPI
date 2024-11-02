using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Phim
{
    public int MaPhim { get; set; }

    public string GhiChu { get; set; } = null!;

    public int DoDaiGio { get; set; }

    public int DoDaiPhut { get; set; }

    public DateTime NgayBatDau { get; set; }

    public string AnhDaiDien { get; set; } = null!;

    public string MoTaPhim { get; set; } = null!;

    public DateTime NgayKetThuc { get; set; }

    public int MaLoaiPhim { get; set; }

    public int MaDangPhim { get; set; }

    public int MaXuatChieu { get; set; }

    public string NoiDungPhim { get; set; } = null!;

    public int DoTuoi { get; set; }

    public int IdQuocGia { get; set; }

    public virtual ICollection<DanhSachDatVeOnline> DanhSachDatVeOnlines { get; set; } = new List<DanhSachDatVeOnline>();

    public virtual QuocGium IdQuocGiaNavigation { get; set; } = null!;

    public virtual DangPhim MaDangPhimNavigation { get; set; } = null!;

    public virtual LoaiPhim MaLoaiPhimNavigation { get; set; } = null!;

    public virtual XuatChieu MaXuatChieuNavigation { get; set; } = null!;
}
