using PetShop.Entities;
using PetShop.Models;

namespace PetShop.Mappers;

public static class ProductMappers
{
    public static Product ToEntity(this ProductRequest dto) => new()
    {
        Name = dto.Name,
        Image = dto.Image,
        Description = dto.Description,
        Variants = dto.Variants.Select(v => v.ToEntity()).ToList()
    };

    public static void UpdateEntity(this Product entity, ProductRequest dto)
    {
        entity.Name = dto.Name;
        entity.Image = dto.Image;
        entity.Description = dto.Description;
        entity.Variants = dto.Variants.Select(v => v.ToEntity()).ToList();
    }

    public static ProductResponse ToDto(this Product entity) =>
        new(entity.Id, entity.Name, entity.Image, entity.Description, entity.Variants.Select(v => v.ToDto()).ToList());
}
