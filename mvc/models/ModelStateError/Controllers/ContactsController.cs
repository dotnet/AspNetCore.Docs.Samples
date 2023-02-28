using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelStateError.Data;
using ModelStateError.Models;

namespace ModelStateError.Controllers;

public class ContactsController : Controller
{
    private readonly ModelStateErrorContext _context;

    public ContactsController(ModelStateErrorContext context)
    {
        _context = context;
    }

    // GET: Contacts
    public async Task<IActionResult> Index()
    {
        return _context.Contact != null ?
                    View(await _context.Contact.ToListAsync()) :
                    Problem("Entity set 'ModelStateErrorContext.Contact'  is null.");
    }

    // GET: Contacts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Contact == null)
        {
            return NotFound();
        }

        var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        return View(contact);
    }

    // GET: Contacts/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Contacts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // <snippet_4>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber")] Contact contact)
    {
        if (_context.Contact.Any(i => i.PhoneNumber == contact.PhoneNumber))
        {
            ModelState.AddModelError(nameof(contact.PhoneNumber), "The Phone number is already in use.");
        }
        if (_context.Contact.Any(i => i.Email == contact.Email))
        {
            ModelState.AddModelError(nameof(contact.Email), "The Email is already in use.");
        }
        if (ModelState.IsValid)
        {
            _context.Add(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(contact);
    }
    // </snippet_4>

    // GET: Contacts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Contact == null)
        {
            return NotFound();
        }

        var contact = await _context.Contact.FindAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        return View(contact);
    }

    // POST: Contacts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // <snippet_1>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,PhoneNumber")] Contact contact)
    {
        if (id != contact.Id)
        {
            return NotFound();
        }

        // Attach Validation Error Message to the Model on validation failure.

        if (_context.Contact.Any(i => i.PhoneNumber == contact.PhoneNumber))
        {
            ModelState.AddModelError(nameof(contact.PhoneNumber), "The Phone number is already in use.");
        }

        if (_context.Contact.Any(i => i.Email == contact.Email))
        {
            ModelState.AddModelError(nameof(contact.Email), "The Email is already in use.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(contact);
    }
    // </snippet_1>

    // GET: Contacts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Contact == null)
        {
            return NotFound();
        }

        var contact = await _context.Contact
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contact == null)
        {
            return NotFound();
        }

        return View(contact);
    }

    // <snippet_2>
    // POST: Contacts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Contact == null)
        {
            return Problem("Entity set 'ModelStateErrorContext.Contact'  is null.");
        }
        var contact = await _context.Contact.FindAsync(id);
        if (contact != null)
        {
            _context.Contact.Remove(contact);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    // </snippet_2>


    private bool ContactExists(int id)
    {
        return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
