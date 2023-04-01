using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Subscribers
{
    public interface ISubscriberRepository
    {
        Task<bool> IsExistedEmail(
            string email,
            CancellationToken cancellationToken = default);

        Task<int> NumberSubscribersAsync(
            CancellationToken cancellationToken = default);

        Task<int> NumberSubscribersTodayAsync(
            CancellationToken cancellationToken = default);

        // e.Định nghĩa các phương thức để thực hiện các công việc sau:
        // Đăng ký theo dõi: SubscribeAsync(email)
        Task<bool> SubscribeAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<bool> SubscribeAsync(
            Subscriber subscriber,
            CancellationToken cancellationToken = default);

        // Hủy đăng ký: UnsubscribeAsync(email, reason, voluntary)
        Task<bool> UnsubscribeAsync(
            string email,
            string reason = null,
            bool typeReason = false,
            CancellationToken cancellationToken = default);

        // Chặn một người theo dõi: BlockSubscriberAsync(id, reason, notes)
        Task ToggleBlockSubscriberAsync(
            Subscriber subscriber,
            bool type,
            CancellationToken cancellationToken = default);

        // Xóa một người theo dõi: DeleteSubscriberAsync(id)
        Task DeleteSubscriberAsync(
            int id,
            CancellationToken cancellationToken = default);

        // Tìm người theo dõi bằng ID: GetSubscriberByIdAsync(id)
        Task<Subscriber> GetSubscriberByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<Subscriber> GetCachedSubscriberByIdAsync(
            int subscriberId,
            CancellationToken cancellationToken = default);

        // Tìm người theo dõi bằng email: GetSubscriberByEmailAsync(email)
        Task<Subscriber> GetSubscriberByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        // Tìm danh sách người theo dõi theo nhiều tiêu chí khác nhau, kết quả được phân trang: Task<IPagedList<Subscriber>>SearchSubscribersAsync(pagingParams, keyword, unsubscribed, involuntary). 
        Task<IPagedList<Subscriber>> SearchSubscribersAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task<IPagedList<Subscriber>> GetPagedSubscribersAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        Task<IPagedList<Subscriber>> GetPagedSubscribersAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task SendEmailUnsubscribe(
            string toEmail,
            string linkUnsub = null,
            CancellationToken cancellationToken = default);
	}
}
