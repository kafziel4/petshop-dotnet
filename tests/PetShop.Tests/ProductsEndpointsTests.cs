using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetShop.Data;
using PetShop.DbContexts;
using PetShop.Models;
using PetShop.Tests.Fixtures;
using PetShop.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace PetShop.Tests;

public class ProductsEndpointsTests : IClassFixture<PetShopApplicationFactory>, IAsyncLifetime
{
    private const string ProductPath = "/products";

    private readonly PetShopApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProductsEndpointsTests(PetShopApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.HttpClient;
    }

    public async Task InitializeAsync()
    {
        await _factory.CreateDatabase();
    }

    public async Task DisposeAsync()
    {
        await _factory.DeleteDatabase();
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOkAndProducts()
    {
        var expectedProduct = SeedingService.Products[0];

        var response = await _client.GetAsync(ProductPath);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ProductsResponse>();
        var productsResponse = content.Should().BeAssignableTo<ProductsResponse>().Subject;
        productsResponse.Products.Should().HaveCount(SeedingService.Products.Count);

        var firstProduct = productsResponse.Products.First();
        firstProduct.Id.Should().Be(expectedProduct.Id);
        firstProduct.Name.Should().Be(expectedProduct.Name);
        firstProduct.Image.Should().Be(expectedProduct.Image);
        firstProduct.Description.Should().Be(expectedProduct.Description);
        firstProduct.Variants.Should().HaveCount(expectedProduct.Variants.Count);
        firstProduct.Variants.First().Size.Should().Be(expectedProduct.Variants.First().Size);
        firstProduct.Variants.First().Price.Should().Be(expectedProduct.Variants.First().Price);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnOkAndProduct_WhenIdExists()
    {
        var expectedProduct = SeedingService.Products[0];
        var response = await _client.GetAsync($"{ProductPath}/{expectedProduct.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ProductResponse>();
        var productResponse = content.Should().BeAssignableTo<ProductResponse>().Subject;
        productResponse.Id.Should().Be(expectedProduct.Id);
        productResponse.Name.Should().Be(expectedProduct.Name);
        productResponse.Image.Should().Be(expectedProduct.Image);
        productResponse.Description.Should().Be(expectedProduct.Description);
        productResponse.Variants.Should().HaveCount(expectedProduct.Variants.Count);
        productResponse.Variants.First().Size.Should().Be(expectedProduct.Variants.First().Size);
        productResponse.Variants.First().Price.Should().Be(expectedProduct.Variants.First().Price);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var response = await _client.GetAsync($"{ProductPath}/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedAtRouteAndAddProductInDb_WhenRequestIsValid()
    {
        var request = ProductRequestFaker.Generate();

        var response = await _client.PostAsJsonAsync(ProductPath, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<ProductResponse>();
        var productResponse = content.Should().BeAssignableTo<ProductResponse>().Subject;
        productResponse.Id.Should().BePositive();
        productResponse.Name.Should().Be(request.Name);
        productResponse.Image.Should().Be(request.Image);
        productResponse.Description.Should().Be(request.Description);
        productResponse.Variants.Should().HaveCount(request.Variants.Count);
        productResponse.Variants.First().Size.Should().Be(request.Variants.First().Size);
        productResponse.Variants.First().Price.Should().Be(request.Variants.First().Price);

        response.Headers.Location.Should().Be(new Uri(_client.BaseAddress!, $"{ProductPath}/{productResponse.Id}"));

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetShopDbContext>();

        var insertedProduct = await context.Products.FirstAsync(p => p.Id == productResponse.Id);
        insertedProduct.Id.Should().Be(productResponse.Id);
        insertedProduct.Name.Should().Be(request.Name);
        insertedProduct.Image.Should().Be(request.Image);
        insertedProduct.Description.Should().Be(request.Description);
        insertedProduct.Variants.Should().HaveCount(request.Variants.Count);
        insertedProduct.Variants.First().Size.Should().Be(request.Variants.First().Size);
        insertedProduct.Variants.First().Price.Should().Be(request.Variants.First().Price);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnValidationProblem_WhenRequestIsInvalid()
    {
        var request = new ProductRequest(
            string.Empty,
            string.Empty,
            string.Empty,
            []);

        var response = await _client.PostAsJsonAsync(ProductPath, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        content.Should().BeAssignableTo<ValidationProblemDetails>()
            .Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNoContentAndUpdateProductInDb_WhenRequestIsValid()
    {
        var originalProduct = SeedingService.Products[0];
        var request = ProductRequestFaker.Generate();

        var response = await _client.PutAsJsonAsync($"{ProductPath}/{originalProduct.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetShopDbContext>();

        var updatedProduct = await context.Products.FirstAsync(p => p.Id == originalProduct.Id);
        updatedProduct.Id.Should().Be(originalProduct.Id);
        updatedProduct.Name.Should().Be(request.Name);
        updatedProduct.Image.Should().Be(request.Image);
        updatedProduct.Description.Should().Be(request.Description);
        updatedProduct.Variants.Should().HaveCount(request.Variants.Count);
        updatedProduct.Variants.First().Size.Should().Be(request.Variants.First().Size);
        updatedProduct.Variants.First().Price.Should().Be(request.Variants.First().Price);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnValidationProblem_WhenRequestIsInvalid()
    {
        var originalProduct = SeedingService.Products[0];
        var request = new ProductRequest(
            string.Empty,
            string.Empty,
            string.Empty,
            []);

        var response = await _client.PutAsJsonAsync($"{ProductPath}/{originalProduct.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        content.Should().BeAssignableTo<ValidationProblemDetails>()
            .Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var request = ProductRequestFaker.Generate();

        var response = await _client.PutAsJsonAsync($"{ProductPath}/999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContentAndDeleteProductInDb_WhenIdExists()
    {
        var product = SeedingService.Products[0];

        var response = await _client.DeleteAsync($"{ProductPath}/{product.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetShopDbContext>();

        var deletedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        deletedProduct.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var response = await _client.DeleteAsync($"{ProductPath}/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}