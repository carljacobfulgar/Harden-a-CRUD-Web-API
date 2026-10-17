using Microsoft.AspNetCore.Mvc;
using ProductsApi.Exceptions;
using ProductsApi.Models;

namespace ProductsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<ProductDto> Products = new()
    {
        new(1, "Mechanical Keyboard", "KEY-1001", 129.99m, 99.99m),
        new(2, "Ergonomic Mouse", "MOU-2002", 59.99m, null)
    };

    [HttpGet]
    public ActionResult<PagedResponse<ProductDto>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 100) pageSize = 10;

        var totalItems = Products.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var pagedItems = Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var meta = new PagedMeta(page, pageSize, totalItems, totalPages);
        return Ok(new PagedResponse<ProductDto>(pagedItems, meta));
    }

    [HttpGet("{id:int}", Name = nameof(GetById))]
    public ActionResult<ProductDto> GetById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id)
            ?? throw new NotFoundException($"Product with ID {id} was not found.");

        return Ok(product);
    }

    [HttpPost]
    public ActionResult<ProductDto> Create([FromBody] CreateProductRequest request)
    {
        if (Products.Any(p => p.Sku.Equals(request.Sku, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ConflictException($"A product with SKU '{request.Sku}' already exists.");
        }

        var newId = Products.Count == 0 ? 1 : Products.Max(p => p.Id) + 1;
        var product = new ProductDto(newId, request.Name, request.Sku, request.Price, request.SalePrice);
        Products.Add(product);

        return CreatedAtRoute(nameof(GetById), new { id = newId }, product);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProductDto> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var index = Products.FindIndex(p => p.Id == id);
        if (index == -1)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }

        var existingSku = Products[index].Sku;
        var updatedProduct = new ProductDto(id, request.Name, existingSku, request.Price, request.SalePrice);
        Products[index] = updatedProduct;

        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id)
            ?? throw new NotFoundException($"Product with ID {id} was not found.");

        Products.Remove(product);
        return NoContent();
    }
}