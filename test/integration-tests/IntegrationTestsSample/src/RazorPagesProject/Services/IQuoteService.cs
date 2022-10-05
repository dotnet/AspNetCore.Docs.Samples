using System.Threading.Tasks;

namespace RazorPagesProject.Services;

// <snippet1>
public interface IQuoteService
{
    Task<string> GenerateQuote();
}
// </snippet1>
