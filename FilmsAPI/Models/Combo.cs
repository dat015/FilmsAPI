using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Combo
{
    public int ComboId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<ComboDetail>? ComboDetails { get; set; } = new List<ComboDetail>();
}
