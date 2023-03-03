using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelStateError.Models;

namespace ModelStateError
{
    public class CreateModel : PageModel
    {
        private readonly ModelStateError.Data.ModelStateErrorContext _context;

        public CreateModel(ModelStateError.Data.ModelStateErrorContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        // <snippet_1>
        public async Task<IActionResult> OnPostAsync()
        {
            // Attach Validation Error Message to the Model on validation failure.          

            // <snippet_5>
            if (Contact.Name == Contact.ShortName)
            {
                ModelState.AddModelError("Contact.ShortName", 
                                         "Short name can't be the same as Name.");
            }
            // </snippet_5>

            if (_context.Contact.Any(i => i.PhoneNumber == Contact.PhoneNumber))
            {
                ModelState.AddModelError("Contact.PhoneNumber",
                                          "The Phone number is already in use.");
            }
            if (_context.Contact.Any(i => i.Email == Contact.Email))
            {
                ModelState.AddModelError("Contact.Email", "The Email is already in use.");
            }

            if (!ModelState.IsValid || _context.Contact == null || Contact == null)
            {
                // if model is invalid, return the page with the model state errors.
                return Page();
            }
            _context.Contact.Add(Contact);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        // </snippet_1>
    }
}
