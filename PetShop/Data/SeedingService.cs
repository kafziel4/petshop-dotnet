using PetShop.DbContexts;
using PetShop.Entities;

namespace PetShop.Data;

public class SeedingService
{
    private readonly PetShopDbContext _context;

    public static List<Product> Products { get; } =
    [
        new Product
        {
            Id = 1,
            Name = "Areia de Gato Grãos Finos",
            Image = "images/areiadegato.png",
            Description = "Areia Higiênica Viva Verde Grãos Finos para Gatos",
            Variants =
            [
                new ProductVariant
                {
                    Price = 30.5m,
                    Size = "1 kg"
                },
                new ProductVariant
                {
                    Price = 69.9m,
                    Size = "3 kg"
                },
                new ProductVariant
                {
                    Price = 110.99m,
                    Size = "6 kg"
                }
            ]
        },
        new Product
        {
            Id = 2,
            Name = "Areia de Gato Grãos Mistos",
            Image = "images/areiadegato2.png",
            Description = "Areia Higiênica Viva Verde Grãos Mistos para Gatos",
            Variants =
            [
                new ProductVariant
                {
                    Price = 30.5m,
                    Size = "1 kg"
                },
                new ProductVariant
                {
                    Price = 69.9m,
                    Size = "3 kg"
                },
                new ProductVariant
                {
                    Price = 110.99m,
                    Size = "6 kg"
                }
            ]
        },
        new Product
        {
            Id = 3,
            Name = "Ração para Cães Adultos com Excesso de Peso",
            Image = "images/dog-food.png",
            Description = "Ração Royal Canin Veterinary Satiety para Cães Adultos de Porte Pequeno com Excesso de Peso",
            Variants =
            [
                new ProductVariant
                {
                    Price = 124.99m,
                    Size = "1.5 kg"
                },
                new ProductVariant
                {
                    Price = 466.99m,
                    Size = "7.5 kg"
                },
                new ProductVariant
                {
                    Price = 761.9m,
                    Size = "10.5 kg"
                }
            ]
        },
        new Product
        {
            Id = 4,
            Name = "Ração para Cães Sênior de Porte Grande e Gigante",
            Image = "images/dog-food2.png",
            Description = "Ração Premier Fórmula para Cães Sênior de Porte Grande e Gigante Sabor Frango",
            Variants =
            [
                new ProductVariant
                {
                    Price = 77.9m,
                    Size = "5 kg"
                },
                new ProductVariant
                {
                    Price = 123.9m,
                    Size = "10 kg"
                },
                new ProductVariant
                {
                    Price = 261.9m,
                    Size = "15 kg"
                }
            ]
        },
        new Product
        {
            Id = 5,
            Name = "Ração Calopsita para Pássaros",
            Image = "images/bird-food.png",
            Description = "Ração Reino das Aves Gold Mix Calopsita para Pássaros",
            Variants =
            [
                new ProductVariant
                {
                    Price = 23.99m,
                    Size = "310 g"
                },
                new ProductVariant
                {
                    Price = 33.99m,
                    Size = "500 g"
                },
                new ProductVariant
                {
                    Price = 149.99m,
                    Size = "5 kg"
                }
            ]
        },
        new Product
        {
            Id = 6,
            Name = "Alimento para Tartarugas",
            Image = "images/turtle-food.png",
            Description = "Alimento Alcon Club Reptolife para Tartarugas",
            Variants =
            [
                new ProductVariant
                {
                    Price = 19.99m,
                    Size = "30 g"
                },
                new ProductVariant
                {
                    Price = 37.99m,
                    Size = "75 g"
                },
                new ProductVariant
                {
                    Price = 290.99m,
                    Size = "1 kg"
                }
            ]
        },
        new Product
        {
            Id = 7,
            Name = "Ração para Jabutis",
            Image = "images/turtle-food2.png",
            Description = "Ração Extrusada Megazoo para Jabutis",
            Variants =
            [
                new ProductVariant
                {

                    Price = 79.07m,
                    Size = "280 g"
                },
                new ProductVariant
                {

                    Price = 199.9m,
                    Size = "500 g"
                },
                new ProductVariant
                {

                    Price = 329.9m,
                    Size = "1 kg"
                }
            ]
        },
        new Product
        {
            Id = 8,
            Name = "Suplemento para Tartaruga",
            Image = "images/turtle-food3.png",
            Description = "Suplemento Alcon Tartaruga Reptocal",
            Variants =
            [
                new ProductVariant
                {
                    Price = 14.9m,
                    Size = "15 g"
                },
                new ProductVariant
                {
                    Price = 32.9m,
                    Size = "20 g"
                },
                new ProductVariant
                {
                    Price = 49.9m,
                    Size = "50 g"
                }
            ]
        }
    ];

    public SeedingService(PetShopDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (_context.Products.Any())
        {
            return;
        }

        _context.AddRange(Products);
        await _context.SaveChangesAsync();
    }
}
