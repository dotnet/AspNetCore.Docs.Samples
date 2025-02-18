namespace WebApiSample.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

[ApiController]
[Route("products/actionresultoft")]
public class ActionResultOfTProductsController : ControllerBase
{
    private readonly ProductContext _productContext;

    public ActionResultOfTProductsController(ProductContext productContext)
    {
        _productContext = productContext;
    }

    // <snippet_GetByIdActionResultOfT>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetById_ActionResultOfT(int id)
    {
        var product = _productContext.Products.Find(id);
        return product == null ? NotFound() : product;
    }
    // </snippet_GetByIdActionResultOfT>

    // <snippet_CreateAsyncActionResultOfT>
    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> CreateAsync_ActionResultOfT(Product product)
    {
        if (product.Description.Contains("XYZ Widget"))
        {
            return BadRequest();
        }

        _productContext.Products.Add(product);
        await _productContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById_ActionResultOfT), new { id = product.Id }, product);
    }
    // </snippet_CreateAsyncActionResultOfT>
}
