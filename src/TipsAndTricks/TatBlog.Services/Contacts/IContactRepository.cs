using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Contacts
{
    public interface IContactRepository
    {
        Task<bool> AddOrUpdateContactAsync(
            Contact contact,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteContactByIdAsync(
           int id,
           CancellationToken cancellationToken = default);

        Task<IList<ContactItem>> GetContactsAsync(
            CancellationToken cancellationToken = default);

        Task<Contact> GetContactByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<Contact> GetCachedContactByIdAsync(
            int contactId,
            CancellationToken cancellationToken = default);
    }
}
