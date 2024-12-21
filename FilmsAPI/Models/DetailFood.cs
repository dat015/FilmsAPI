using System;
using System.Collections.Generic;

namespace FilmsAPI.Models;

public partial class DetailFood
{
    public int Id { get; set; }

    public int FoodId { get; set; }

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public decimal ThanhTien { get; set; }

    public virtual Food? Food { get; set; } = null!;

    public virtual HoaDon? Order { get; set; } = null!;
}
