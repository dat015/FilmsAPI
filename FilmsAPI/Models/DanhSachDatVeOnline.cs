using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class DanhSachDatVeOnline
{
    public int MaDatVe { get; set; }

    public int MaKhachHang { get; set; }

    public int MaXacNhan { get; set; }

    public int MaPhim { get; set; }

    public int SoLuongVeThuong { get; set; }

    public int SoLuongVeVip { get; set; }

    public string TrangThaiDatVe { get; set; } = null!;

    public int MaXuatChieu { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual Phim MaPhimNavigation { get; set; } = null!;

    public virtual XuatChieu MaXuatChieuNavigation { get; set; } = null!;
}
