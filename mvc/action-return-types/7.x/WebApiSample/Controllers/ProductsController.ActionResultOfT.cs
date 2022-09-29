namespace WebApiSample.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

public partial class ProductsController : ControllerBase
{
#if ActionResultOfT
        // <snippet_GetByIdActionResultOfT>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetById(int id)
        {
            if (!_repository.TryGetProduct(id, out var product))
            {
                return NotFound();
            }

            return product;
        }
        // </snippet_GetByIdActionResultOfT>

        // <snippet_CreateAsyncActionResultOfT>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateAsync(Product product)
        {
            if (product.Description.Contains("XYZ Widget"))
            {
                return BadRequest();
            }

            await _repository.AddProductAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        // </snippet_CreateAsyncActionResultOfT>
#endif
}
