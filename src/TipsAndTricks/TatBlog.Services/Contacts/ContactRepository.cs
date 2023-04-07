using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Contacts
{
    public class ContactRepository : IContactRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public ContactRepository(BlogDbContext blogDbContext, IMemoryCache memoryCache)
        {
            _context = blogDbContext;
            _memoryCache = memoryCache;
        }

        public async Task<bool> AddOrUpdateContactAsync(
            Contact contact,
            CancellationToken cancellationToken = default)
        {
            if (contact.Id > 0)
            {
                _context.Contacts.Update(contact);
            }
            else
            {
                _context.Contacts.Add(contact);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteContactByIdAsync(
           int id,
           CancellationToken cancellationToken = default)
        {
            var contact = await _context.Set<Contact>().FindAsync(id);

            if (contact is null) return false;

            _context.Set<Contact>().Remove(contact);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }

        public async Task<IList<ContactItem>> GetContactsAsync(
            CancellationToken cancellationToken = default)
        {
            IQueryable<Contact> categories = _context.Set<Contact>();

            return await categories
                .OrderBy(x => x.Subject)
                .Select(x => new ContactItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    Subject = x.Subject,
                    Description = x.Description,
                    ContactDate = x.ContactDate
                }).ToListAsync(cancellationToken);
        }

        public async Task<Contact> GetContactByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Contact>()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Contact> GetCachedContactByIdAsync(
            int contactId,
            CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"contact.by-id.{contactId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetContactByIdAsync(contactId, cancellationToken);
                });
        }
    }
}
