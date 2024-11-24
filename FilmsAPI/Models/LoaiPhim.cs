using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class LoaiPhim
{
    public int MaTheLoai { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public virtual ICollection<TheLoaiCuaPhim>? TheLoaiCuaPhims { get; set; } = new List<TheLoaiCuaPhim>();
}
