namespace WebApiSample.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;
using WebApiSample.Repositories;

[ApiController]
[Route("[controller]")]
public partial class ProductsController : ControllerBase
{
    private readonly ProductsRepository _repository;

    public ProductsController(ProductsRepository repository)
    {
        _repository = repository;
    }

    // <snippet_Get>
    [HttpGet]
    public List<Product> Get() =>
        _repository.GetProducts();
    // </snippet_Get>

    // <snippet_GetOnSaleProducts>
    [HttpGet("syncsale")]
    public IEnumerable<Product> GetOnSaleProducts()
    {
        var products = _repository.GetProducts();

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
        var products = _repository.GetProductsAsync();

        await foreach (var product in products)
        {
            if (product.IsOnSale)
            {
                yield return product;
            }
        }
    }
    // </snippet_GetOnSaleProductsAsync>
}
