using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class TheLoaiCuaPhim
{
    public int Maphim { get; set; }

    public int MaTheLoai { get; set; }

    public int Id { get; set; }

    public virtual LoaiPhim? MaTheLoaiNavigation { get; set; } = null!;

    public virtual Phim? MaphimNavigation { get; set; } = null!;
}
