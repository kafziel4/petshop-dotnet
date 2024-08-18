using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PetShop.DbContexts;
using PetShop.Mappers;
using PetShop.Models;

namespace PetShop.EndpointHandlers;

public static class ProductsHandler
{
    public static async Task<Ok<ProductsResponse>> GetProductsAsync(
        PetShopDbContext context)
    {
        var products = await context.Products
            .Include(p => p.Variants)
            .Select(p => p.ToDto())
            .ToListAsync();

        return TypedResults.Ok(new ProductsResponse(products));
    }

    public static async Task<Results<NotFound, Ok<ProductResponse>>> GetProductByIdAsync(PetShopDbContext context,
        int id)
    {
        var product = await context.Products
            .Include(p => p.Variants)
            .Where(p => p.Id == id)
            .Select(p => p.ToDto())
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(product);
    }

    public static async Task<Results<ValidationProblem, CreatedAtRoute<ProductResponse>>> CreateProductAsync(
        IValidator<ProductRequest> validator,
        PetShopDbContext context,
        ProductRequest request)
    {
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        var product = request.ToEntity();
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return TypedResults.CreatedAtRoute(
            product.ToDto(),
            nameof(GetProductByIdAsync),
            new { id = product.Id });
    }

    public static async Task<Results<ValidationProblem, NotFound, NoContent>> UpdateProductAsync(
        IValidator<ProductRequest> validator,
        PetShopDbContext context,
        int id,
        ProductRequest request)
    {
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        var product = await context.Products
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return TypedResults.NotFound();
        }

        product.UpdateEntity(request);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public static async Task<Results<NotFound, NoContent>> DeleteProductAsync(
        PetShopDbContext context,
        int id)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return TypedResults.NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}
