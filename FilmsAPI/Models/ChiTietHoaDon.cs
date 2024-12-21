    using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class ChiTietHoaDon
{
    public int MaHd { get; set; }

    public int MaVe { get; set; }

    public int SoLuong { get; set; }

    public decimal ThanhTien { get; set; }

    public virtual HoaDon? MaHdNavigation { get; set; } = null!;

    public virtual Ve? MaVeNavigation { get; set; } = null!;
}
