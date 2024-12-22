using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class ComboDetail
{
    public int ComboDetailsId { get; set; }

    public int ComboId { get; set; }

    public int FoodId { get; set; }

    public virtual Combo? Combo { get; set; } = null!;

    public virtual Food? Food { get; set; } = null!;
}
