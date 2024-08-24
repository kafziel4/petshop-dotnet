using PetShop.EndpointHandlers;

namespace PetShop.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterProductsEndpoints(this IEndpointRouteBuilder builder)
    {
        var productsEndpoints = builder.MapGroup("/products");
        var productWithIdEndpoints = productsEndpoints.MapGroup("/{id:int}");

        productsEndpoints.MapGet("", ProductsHandler.GetProductsAsync);
        productsEndpoints.MapPost("", ProductsHandler.CreateProductAsync);

        productWithIdEndpoints.MapGet("", ProductsHandler.GetProductByIdAsync)
            .WithName(nameof(ProductsHandler.GetProductByIdAsync));
        productWithIdEndpoints.MapPut("", ProductsHandler.UpdateProductAsync);
        productWithIdEndpoints.MapDelete("", ProductsHandler.DeleteProductAsync);
    }
}
