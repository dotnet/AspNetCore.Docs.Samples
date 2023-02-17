using IResultImplementation.Data;
using IResultImplementation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IResultImplementation
{
    public static class ContactsHandler
    {

        // GET: api/Contacts
        public static IResult GetContacts(IResultImplementationContext context)
        {
            return TypedResults.Ok(context.Contact.ToList());
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public static IResult GetContact(IResultImplementationContext context, int id)
        {
            var contact = context.Contact.Where(c => c.Id == id).FirstOrDefault();

            if (contact == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(contact);
        }


        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public static IResult PostContact(IResultImplementationContext context, Contact contact)
        {
            context.Contact.Add(contact);
            context.SaveChanges();

            return TypedResults.CreatedAtRoute<Contact>(contact, nameof(ContactsHandler.GetContact));
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public static IResult PutContact(IResultImplementationContext context, int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return TypedResults.BadRequest();
            }

            context.Entry(contact).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(context, id))
                {
                    return TypedResults.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return TypedResults.NoContent();
        }


        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public static IResult DeleteContact(IResultImplementationContext context, int id)
        {
            if (context.Contact == null)
            {
                return TypedResults.NotFound();
            }
            var contact = context.Contact.Find(id);
            if (contact == null)
            {
                return TypedResults.NotFound();
            }

            context.Contact.Remove(contact);
            context.SaveChanges();

            return TypedResults.NoContent();
        }

        private static bool ContactExists(IResultImplementationContext context, int id)
        {
            return (context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
