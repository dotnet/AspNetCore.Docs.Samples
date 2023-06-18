namespace WebApiSample.Models;

using System.ComponentModel.DataAnnotations;

// <snippet_ProductClass>
public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public bool IsOnSale { get; set; }
}
// </snippet_ProductClass>
