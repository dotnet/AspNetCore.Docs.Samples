namespace WebApiSample.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Models;

public partial class ProductsController : ControllerBase
{
#if IActionResult
        // <snippet_GetByIdIActionResult>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            if (!_repository.TryGetProduct(id, out var product))
            {
                return NotFound();
            }

            return Ok(product);
        }
        // </snippet_GetByIdIActionResult>

        // <snippet_CreateAsyncIActionResult>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            if (product.Description.Contains("XYZ Widget"))
            {
                return BadRequest();
            }

            await _repository.AddProductAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        // </snippet_CreateAsyncIActionResult>
#endif
}
