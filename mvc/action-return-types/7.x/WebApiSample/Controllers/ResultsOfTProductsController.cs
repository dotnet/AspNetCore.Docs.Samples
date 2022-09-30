namespace WebApiSample.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

[ApiController]
[Route("products/resultsoft")]
public class ResultsOfTProductsController : ControllerBase
{
    private readonly ProductContext _productContext;

    public ResultsOfTProductsController(ProductContext productContext)
    {
        _productContext = productContext;
    }

    // <snippet_GetByIdResultsOfT>
    [HttpGet("{id}")]
    public Results<NotFound, Ok<Product>> GetById(int id)
    {
        var product = _productContext.Products.Find(id);
        return product == null ? TypedResults.NotFound() : TypedResults.Ok(product);
    }
    // </snippet_GetByIdResultsOfT>

    // <snippet_CreateAsyncResultsOfT>
    [HttpPost]
    public async Task<Results<BadRequest, Created<Product>>> CreateAsync(Product product)
    {
        if (product.Description.Contains("XYZ Widget"))
        {
            return TypedResults.BadRequest();
        }

        _productContext.Products.Add(product);
        await _productContext.SaveChangesAsync();

        var location = Url.Action(nameof(GetById), new { id = product.Id }) ?? $"/{product.Id}";
        return TypedResults.Created(location, product);
    }
    // </snippet_CreateAsyncResultsOfT>
}
