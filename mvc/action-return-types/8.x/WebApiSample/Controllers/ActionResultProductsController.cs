namespace WebApiSample.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

[ApiController]
[Route("products/iactionresult")]
public class ActionResultProductsController : ControllerBase
{
    private readonly ProductContext _productContext;

    public ActionResultProductsController(ProductContext productContext)
    {
        _productContext = productContext;
    }

    // <snippet_GetByIdIActionResult>
    [HttpGet("{id}")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById_IActionResult(int id)
    {
        var product = _productContext.Products.Find(id);
        return product == null ? NotFound() : Ok(product);
    }
    // </snippet_GetByIdIActionResult>

    // <snippet_CreateAsyncIActionResult>
    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync_IActionResult(Product product)
    {
        if (product.Description.Contains("XYZ Widget"))
        {
            return BadRequest();
        }

        _productContext.Products.Add(product);
        await _productContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById_IActionResult), new { id = product.Id }, product);
    }
    // </snippet_CreateAsyncIActionResult>
}
