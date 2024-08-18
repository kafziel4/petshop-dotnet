using FluentValidation;
using PetShop.Entities;
using PetShop.Models;

namespace PetShop.Validators;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(Product.MaxNameLength)
            .WithMessage($"Name must be less than {Product.MaxNameLength} characters.");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Image is required.")
            .MaximumLength(Product.MaxImageLength)
            .WithMessage($"Image must be less than {Product.MaxImageLength} characters.")
            .Must(Product.IsValidImageFile)
            .WithMessage($"Image must have one of the following extensions: {string.Join(", ", Product.PermittedExtensions)}");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(Product.MaxDescriptionLength)
            .WithMessage($"Description must be less than {Product.MaxDescriptionLength} characters.");

        RuleFor(x => x.Variants)
            .NotEmpty()
            .WithMessage("Variants are required.");

        RuleForEach(x => x.Variants).ChildRules(variant =>
        {
            variant.RuleFor(x => x.Size)
            .NotEmpty()
            .WithMessage("Size is required.")
            .MaximumLength(ProductVariant.MaxSizeLength)
            .WithMessage($"Size must be less than {ProductVariant.MaxSizeLength} characters.");

            variant.RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be greater than or equal to 0.")
                .PrecisionScale(ProductVariant.PricePrecision, ProductVariant.PriceScale, false)
                .WithMessage($"Price must have a maximum of {ProductVariant.PricePrecision - ProductVariant.PriceScale} digits and {ProductVariant.PriceScale} decimal places.");
        });
    }
}
