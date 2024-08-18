namespace PetShop.Models;

public record ProductsResponse(ICollection<ProductResponse> Products);

public record ProductResponse(
    int Id, string Name, string Image, string Description, ICollection<ProductVariantResponse> Variants);

public record ProductRequest(
    string Name, string Image, string Description, ICollection<ProductVariantRequest> Variants);
