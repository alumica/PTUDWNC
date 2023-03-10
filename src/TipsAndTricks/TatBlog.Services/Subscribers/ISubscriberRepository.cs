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


        // e.Định nghĩa các phương thức để thực hiện các công việc sau:
        // Đăng ký theo dõi: SubscribeAsync(email)
        Task<Subscriber> SubscribeAsync(
            string email,
            CancellationToken cancellationToken = default);

        // Hủy đăng ký: UnsubscribeAsync(email, reason, voluntary)
        Task UnsubscribeAsync(
            string email,
            string reason,
            bool typeReason,
            CancellationToken cancellationToken = default);

        // Chặn một người theo dõi: BlockSubscriberAsync(id, reason, notes)
        Task BlockSubscriberAsync(
            int id,
            string reason,
            string notes,
            CancellationToken cancellationToken = default);

        // Xóa một người theo dõi: DeleteSubscriberAsync(id)
        Task DeleteSubscriberAsync(
            int id,
            CancellationToken cancellationToken = default);

        // Tìm người theo dõi bằng ID: GetSubscriberByIdAsync(id)
        Task<Subscriber> GetSubscriberByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // Tìm người theo dõi bằng email: GetSubscriberByEmailAsync(email)
        Task<Subscriber> GetSubscriberByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        // Tìm danh sách người theo dõi theo nhiều tiêu chí khác nhau, kết quả được phân trang: Task<IPagedList<Subscriber>>SearchSubscribersAsync(pagingParams, keyword, unsubscribed, involuntary). 
        Task<IPagedList<Subscriber>> SearchSubscribersAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
    }
}
