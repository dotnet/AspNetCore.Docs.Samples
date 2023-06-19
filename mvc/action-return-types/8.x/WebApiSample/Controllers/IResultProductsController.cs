namespace WebApiSample.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

[ApiController]
[Route("products/iresult")]
public class IResultProductsController : ControllerBase
{
    private readonly ProductContext _productContext;

    public IResultProductsController(ProductContext productContext)
    {
        _productContext = productContext;
    }

    // <snippet_GetByIdIResult>
    [HttpGet("{id}")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetById(int id)
    {
        var product = _productContext.Products.Find(id);
        return product == null ? Results.NotFound() : Results.Ok(product);
    }
    // </snippet_GetByIdIResult>

    // <snippet_CreateAsyncIResult>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Product>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateAsync(Product product)
    {
        if (product.Description.Contains("XYZ Widget"))
        {
            return Results.BadRequest();
        }

        _productContext.Products.Add(product);
        await _productContext.SaveChangesAsync();

        var location = Url.Action(nameof(GetById), new { id = product.Id }) ?? $"/{product.Id}";
        return Results.Created(location, product);
    }
    // </snippet_CreateAsyncIResult>
}
