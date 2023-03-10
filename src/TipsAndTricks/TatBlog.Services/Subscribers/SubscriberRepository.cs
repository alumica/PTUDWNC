using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Subscribers
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Subscriber> SubscribeAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            Subscriber s = null;
            if (!string.IsNullOrWhiteSpace(email) && !IsExistedEmail(email).Result)
            {
                s = new Subscriber()
                {
                    Email = email,
                    SubscribeDate = DateTime.Now,
                };
                _context.Subscribers.Add(s);
                _context.SaveChanges();
            }
            return s;
        }

        public async Task UnsubscribeAsync(
            string email,
            string reason, 
            bool typeReason, 
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(email) && IsExistedEmail(email).Result)
            {
                await _context.Set<Subscriber>()
                .Where(s => s.Email.Equals(email))
                .ExecuteUpdateAsync(p => p
                    .SetProperty(x => x.ResonUnsubscribe, x => reason)
                    .SetProperty(x => x.TypeReason, x => typeReason),
                    cancellationToken);
            }

        }

        public async Task BlockSubscriberAsync(
            int id, 
            string reason,
            string notes,
            CancellationToken cancellationToken = default)
        {
            if (id > 0)
            {
                await _context.Set<Subscriber>()
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(x => x.ResonUnsubscribe, x => reason)
                    .SetProperty(x => x.TypeReason, x => false)
                    .SetProperty(x => x.Notes, x => notes),
                    cancellationToken);
            }
        }

        public async Task DeleteSubscriberAsync(
            int id, 
            CancellationToken cancellationToken = default)
        {
            //await _context.Database
            //   .ExecuteSqlRawAsync("DELETE FROM Subscribers WHERE Id = " + id, cancellationToken);

            await _context.Set<Subscriber>()
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<Subscriber> GetSubscriberByEmailAsync(
            string email, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .Where(s => s.Email.Equals(email))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Subscriber> GetSubscriberByIdAsync(
            int id, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<IPagedList<Subscriber>> SearchSubscribersAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistedEmail(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .AnyAsync(s => s.Email.Equals(email), cancellationToken);
        }
    }
}
