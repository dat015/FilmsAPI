using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FilmsAPI.Models;

public partial class DangPhim
{
    public int MaDangPhim { get; set; }

    public string TenDangPhim { get; set; } = null!;

    public int MaManHinh { get; set; }
    public virtual ManHinh? MaManHinhNavigation { get; set; } = null!;

    public virtual ICollection<Phim>? Phims { get; set; } = new List<Phim>();
}
