namespace ProductsApi.Models;

public record ProductDto(
    int Id,
    string Name,
    string Sku,
    decimal Price,
    decimal? SalePrice
);

public record PagedMeta(
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);

public record PagedResponse<T>(
    IEnumerable<T> Data,
    PagedMeta Meta
);