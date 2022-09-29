namespace WebApiSample.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

public partial class ProductsController : ControllerBase
{
#if ResultsOfT
    // <snippet_GetByIdResultsOfT>
    [HttpGet("{id}")]
    public Results<NotFound, Ok<Product>> GetById(int id)
    {
        if (!_repository.TryGetProduct(id, out var product))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(product);
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

        await _repository.AddProductAsync(product);

        var location = Url.Action(nameof(GetById), new { id = product.Id }) ?? $"/{product.Id}";
        return TypedResults.Created(location, product);
    }
    // </snippet_CreateAsyncResultsOfT>
#endif
}
