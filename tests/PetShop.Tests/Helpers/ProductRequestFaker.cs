using Bogus;
using PetShop.Entities;
using PetShop.Models;

namespace PetShop.Tests.Helpers;

public static class ProductRequestFaker
{
    public static ProductRequest Generate()
    {
        var faker = new Faker<ProductRequest>();

        faker.CustomInstantiator(f =>
            new ProductRequest(
                f.Commerce.ProductName(),
                f.System.FileName(f.Random.ArrayElement(Product.PermittedExtensions)[1..]),
                f.Lorem.Paragraph(),
                f.Make(f.Random.Number(1, 5), () => new ProductVariantRequest(
                    f.Commerce.ProductAdjective(),
                    Math.Round(f.Random.Decimal(0, 1_000_000_000), 2)
                ))
            ));

        return faker.Generate();
    }
}
