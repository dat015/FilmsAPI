using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class QuocGia
{
    public int IdQuocGia { get; set; }

    public string TenNuoc { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}
