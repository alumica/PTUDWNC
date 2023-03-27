using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Subscribers
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<int> NumberSubscribersAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .CountAsync(cancellationToken);
        }

        public async Task<int> NumberSubscribersTodayAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Subscriber>()
                .CountAsync(x => x.SubscribeDate.CompareTo(DateTime.Now) == 0, cancellationToken);
        }


        public async Task<bool> SubscribeAsync(
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
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            return false;
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

        public async Task ToggleBlockSubscriberAsync(
            Subscriber subscriber,
            bool type,
            CancellationToken cancellationToken = default)
        {
            if (type)
            {
                await _context.Set<Subscriber>()
                .Where(s => s.Id == subscriber.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(x => x.ResonUnsubscribe, x => subscriber.ResonUnsubscribe)
                    .SetProperty(x => x.TypeReason, x => false)
                    .SetProperty(x => x.Notes, x => subscriber.Notes),
                    cancellationToken);
            }
            else
            {
                await _context.Set<Subscriber>()
                .Where(s => s.Id == subscriber.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(x => x.ResonUnsubscribe, x => "")
                    .SetProperty(x => x.TypeReason, x => null)
                    .SetProperty(x => x.Notes, x => ""),
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

        public async Task<IPagedList<Subscriber>> GetPagedSubscribersAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Subscriber> subscriberQuery = _context.Set<Subscriber>();

            return await subscriberQuery.ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Subscriber.Email), "DESC",
                cancellationToken);
        }

        public async Task SendEmailUnsubscribe(
            string toEmail,
            string linkUnsub = null,
            CancellationToken cancellationToken = default)
        {
            var sender = new SmtpSender(() => new SmtpClient("localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25
            });

            StringBuilder template = new StringBuilder();
            template.AppendLine("Kính gửi @Model.Email");
            template.AppendLine("<p>Cảm ơn bạn đã đăng ký nhận tin của TatBlog.</p>");
			template.AppendLine("<p>(Nễu muốn hủy nhận tin hãy bấm vào <a>đây</a></p>");

            Email.DefaultSender = sender;
            Email.DefaultRenderer = new RazorRenderer();

            var email = await Email
                .From("2011400@tatblog.com")
                .To(toEmail)
                .Subject("Thanks")
                .UsingTemplate(template.ToString(), new { Email = toEmail })
                .SendAsync(cancellationToken);
		}
    }
}
