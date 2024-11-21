using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class LoaiVe
{
    public int MaLoai { get; set; }

    public string TenLoaiVe { get; set; } = null!;

    public virtual ICollection<Ve>? Ves { get; set; } = new List<Ve>();
}
