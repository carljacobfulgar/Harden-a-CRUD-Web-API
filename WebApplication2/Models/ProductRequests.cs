using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Models;

public record CreateProductRequest(
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    string Name,

    [Required(ErrorMessage = "SKU is required.")]
    [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "SKU must match format 'AAA-1234'.")]
    string Sku,

    [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00.")]
    decimal Price,

    [Range(0.00, 10000.00, ErrorMessage = "Sale price cannot be negative or exceed 10,000.00.")]
    decimal? SalePrice
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalePrice.HasValue && SalePrice >= Price)
        {
            yield return new ValidationResult(
                "Sale price must be strictly less than the regular price.",
                new[] { nameof(SalePrice) }
            );
        }
    }
}

public record UpdateProductRequest(
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    string Name,

    [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00.")]
    decimal Price,

    [Range(0.00, 10000.00, ErrorMessage = "Sale price cannot be negative or exceed 10,000.00.")]
    decimal? SalePrice
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalePrice.HasValue && SalePrice >= Price)
        {
            yield return new ValidationResult(
                "Sale price must be strictly less than the regular price.",
                new[] { nameof(SalePrice) }
            );
        }
    }
}