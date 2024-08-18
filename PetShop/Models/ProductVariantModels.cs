namespace PetShop.Models;

public record ProductVariantResponse(string Size, decimal Price);

public record ProductVariantRequest(string Size, decimal Price);
