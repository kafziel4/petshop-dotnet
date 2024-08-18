using FluentValidation.TestHelper;
using PetShop.Entities;
using PetShop.Models;
using PetShop.Tests.Helpers;
using PetShop.Validators;

namespace PetShop.Tests.Validators;

public class ProductRequestValidatorTests
{
    private readonly ProductRequestValidator _validator = new();
    private readonly ProductRequest _request = ProductRequestFaker.Generate();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = _request with { Name = string.Empty };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var model = _request with { Name = new string('a', Product.MaxNameLength + 1) };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage($"Name must be less than {Product.MaxNameLength} characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Image_Is_Empty()
    {
        var model = _request with { Image = string.Empty };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Image)
            .WithErrorMessage("Image is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Image_Exceeds_MaxLength()
    {
        var model = _request with { Image = new string('a', Product.MaxImageLength + 1) };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Image)
            .WithErrorMessage($"Image must be less than {Product.MaxImageLength} characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Image_Is_Invalid()
    {
        var model = _request with { Image = "invalid_image.txt" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Image)
            .WithErrorMessage($"Image must have one of the following extensions: {string.Join(", ", Product.PermittedExtensions)}");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = _request with { Description = string.Empty };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var model = _request with { Description = new string('a', Product.MaxDescriptionLength + 1) };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage($"Description must be less than {Product.MaxDescriptionLength} characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Variants_Are_Empty()
    {
        var model = _request with { Variants = [] };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Variants)
            .WithErrorMessage("Variants are required.");
    }

    [Fact]
    public void Should_Have_Error_When_Variant_Size_Is_Empty()
    {
        var model = _request with
        {
            Variants =
            [
                _request.Variants.First() with { Size = string.Empty }
            ]
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor("Variants[0].Size")
            .WithErrorMessage("Size is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Variant_Size_Exceeds_MaxLength()
    {
        var model = _request with
        {
            Variants =
            [
                _request.Variants.First() with { Size = new string('a', ProductVariant.MaxSizeLength + 1) }
            ]
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor("Variants[0].Size")
            .WithErrorMessage($"Size must be less than {ProductVariant.MaxSizeLength} characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Variant_Price_Is_Negative()
    {
        var model = _request with
        {
            Variants =
            [
                _request.Variants.First() with { Price = -1 }
            ]
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor("Variants[0].Price")
            .WithErrorMessage("Price must be greater than or equal to 0.");
    }

    [Fact]
    public void Should_Have_Error_When_Variant_Price_Has_Invalid_Precision_Or_Scale()
    {
        var model = _request with
        {
            Variants =
            [
                _request.Variants.First() with { Price = 1234567890.123m }
            ]
        };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor("Variants[0].Price")
            .WithErrorMessage($"Price must have a maximum of {ProductVariant.PricePrecision - ProductVariant.PriceScale} digits and {ProductVariant.PriceScale} decimal places.");
    }
}
