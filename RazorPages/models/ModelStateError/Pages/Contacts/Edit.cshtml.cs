using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelStateError.Data;
using ModelStateError.Models;

namespace ModelStateError
{
    public class EditModel : PageModel
    {
        private readonly ModelStateError.Data.ModelStateErrorContext _context;

        public EditModel(ModelStateError.Data.ModelStateErrorContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Contact Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Attach Validation Error Message to the Model on validation failure.
            
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            if (_context.Contact.Any(i => i.PhoneNumber == Contact.PhoneNumber))
            {
                ModelState.AddModelError(nameof(Contact.PhoneNumber), "The Phone number is already in use.");
            }
            if (_context.Contact.Any(i => i.Email == Contact.Email))
            {
                ModelState.AddModelError(nameof(Contact.Email), "The Email is already in use.");
            }
            if (Contact.Name == Contact.ShortName)
            {
                ModelState.AddModelError(nameof(Contact.ShortName), "Short name can't be the same as Name.");
            }
            var contact =  await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            Contact = contact;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(Contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ContactExists(int id)
        {
          return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
