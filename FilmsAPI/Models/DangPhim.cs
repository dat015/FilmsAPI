using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class DangPhim
{
    public int MaDangPhim { get; set; }

    public string TenDangPhim { get; set; } = null!;

    public virtual ICollection<Gium> Gia { get; set; } = new List<Gium>();

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}
