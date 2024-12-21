using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class Food
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int CateId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Category? Cate { get; set; } = null!;

    public virtual ICollection<ComboDetail>? ComboDetails { get; set; } = new List<ComboDetail>();

    public virtual ICollection<DetailFood>? DetailFoods { get; set; } = new List<DetailFood>();
}
