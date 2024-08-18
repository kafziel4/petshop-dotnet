using System.ComponentModel.DataAnnotations;

namespace PetShop.Entities;

public class Product
{
    public const int MaxNameLength = 200;
    public const int MaxImageLength = 255;
    public const int MaxDescriptionLength = 800;

    public static string[] PermittedExtensions { get; } =
        [".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp"];

    public int Id { get; set; }

    [MaxLength(MaxNameLength)]
    public required string Name { get; set; }

    [MaxLength(MaxImageLength)]
    public required string Image { get; set; }

    [MaxLength(MaxDescriptionLength)]
    public required string Description { get; set; }

    public ICollection<ProductVariant> Variants { get; set; } = [];

    public static bool IsValidImageFile(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();

        if (string.IsNullOrEmpty(extension) || !PermittedExtensions.Contains(extension))
        {
            return false;
        }

        return true;
    }
}