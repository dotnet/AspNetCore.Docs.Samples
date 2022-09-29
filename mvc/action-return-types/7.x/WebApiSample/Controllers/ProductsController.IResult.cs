namespace WebApiSample.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

public partial class ProductsController : ControllerBase
{
#if IResult
    // <snippet_GetByIdIResult>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetById(int id)
    {
        if (!_repository.TryGetProduct(id, out var product))
        {
            return Results.NotFound();
        }

        return Results.Ok(product);
    }
    // </snippet_GetByIdIResult>

    // <snippet_CreateAsyncIResult>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateAsync(Product product)
    {
        if (product.Description.Contains("XYZ Widget"))
        {
            return Results.BadRequest();
        }

        await _repository.AddProductAsync(product);

        var location = Url.Action(nameof(GetById), new { id = product.Id }) ?? $"/{product.Id}";
        return Results.Created(location, product);
    }
    // </snippet_CreateAsyncIResult>
#endif
}
