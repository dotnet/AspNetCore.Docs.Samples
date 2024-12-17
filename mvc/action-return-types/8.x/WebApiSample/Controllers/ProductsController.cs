namespace WebApiSample.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Models;

[ApiController]
[Route("[controller]")]
public partial class ProductsController : ControllerBase
{
    private readonly ProductContext _productContext;

    public ProductsController(ProductContext productContext)
    {
        _productContext = productContext;
    }

    // <snippet_Get>
    [HttpGet]
    public Task<List<Product>> Get() =>
        _productContext.Products.OrderBy(p => p.Name).ToListAsync();
    // </snippet_Get>

    // <snippet_GetOnSaleProducts>
    [HttpGet("syncsale")]
    public IEnumerable<Product> GetOnSaleProducts()
    {
        var products = _productContext.Products.OrderBy(p => p.Name).ToList();

        foreach (var product in products)
        {
            if (product.IsOnSale)
            {
                yield return product;
            }
        }
    }
    // </snippet_GetOnSaleProducts>

    // <snippet_GetOnSaleProductsAsync>
    [HttpGet("asyncsale")]
    public async IAsyncEnumerable<Product> GetOnSaleProductsAsync()
    {
        var products = _productContext.Products.OrderBy(p => p.Name).AsAsyncEnumerable();

        await foreach (var product in products)
        {
            if (product.IsOnSale)
            {
                yield return product;
            }
        }
    }
    // </snippet_GetOnSaleProductsAsync>

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        var product = await _productContext.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }
}
