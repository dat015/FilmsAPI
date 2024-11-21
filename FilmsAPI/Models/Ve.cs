using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Ve
{
    public int MaVe { get; set; }

    public decimal GiaVe { get; set; }

    public bool TrangThai { get; set; }

    public int MaLoaiVe { get; set; }

    public int MaGhe { get; set; }

    public int MaXuatChieu { get; set; }

    public int MaKh { get; set; }

    public int MaNv { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual Ghe? MaGheNavigation { get; set; } = null!;

    public virtual KhachHang? MaKhNavigation { get; set; } = null!;

    public virtual LoaiVe? MaLoaiVeNavigation { get; set; } = null!;

    public virtual NhanVien? MaNvNavigation { get; set; } = null!;

    public virtual XuatChieu? MaXuatChieuNavigation { get; set; } = null!;
}
