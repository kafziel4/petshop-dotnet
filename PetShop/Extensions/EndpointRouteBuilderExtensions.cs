using PetShop.EndpointHandlers;

namespace PetShop.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterProductsEndpoints(this IEndpointRouteBuilder builder)
    {
        var productsEndpoints = builder.MapGroup("/products");
        var productWithIdEndpoints = productsEndpoints.MapGroup("/{id:int}");

        productsEndpoints.MapGet("", ProductsHandler.GetProductsAsync)
            .WithOpenApi();
        productsEndpoints.MapPost("", ProductsHandler.CreateProductAsync)
            .WithOpenApi();

        productWithIdEndpoints.MapGet("", ProductsHandler.GetProductByIdAsync)
            .WithName(nameof(ProductsHandler.GetProductByIdAsync))
            .WithOpenApi();
        productWithIdEndpoints.MapPut("", ProductsHandler.UpdateProductAsync)
            .WithOpenApi();
        productWithIdEndpoints.MapDelete("", ProductsHandler.DeleteProductAsync)
            .WithOpenApi();
    }
}
