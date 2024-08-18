using PetShop.Entities;
using PetShop.Models;

namespace PetShop.Mappers;

public static class ProductVariantMappers
{
    public static ProductVariant ToEntity(this ProductVariantRequest dto) => new()
    {
        Size = dto.Size,
        Price = dto.Price,
    };

    public static ProductVariantResponse ToDto(this ProductVariant entity) =>
        new(entity.Size, entity.Price);
}
