using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Entities;

public class ProductVariant
{
    public const int MaxSizeLength = 20;
    public const int PricePrecision = 12;
    public const int PriceScale = 2;

    [MaxLength(MaxSizeLength)]
    public required string Size { get; set; }

    [Precision(PricePrecision, PriceScale)]
    public decimal Price { get; set; }
}
