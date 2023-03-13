using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Authors
{
    public interface IAuthorRepository
    {
        // 2. Tạo các lớp và định nghĩa các phương thức
        // cần thiết để truy vấn và cập nhật thông tin tác giả bài viết.
        // 2.a. Tạo interface IAuthorRepository và lớp AuthorRepository.

        // 2.b. Tìm một tác giả theo mã số.
        Task<Author> FindAuthorByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // 2.c. Tìm một tác giả theo tên định danh(slug). 
        Task<Author> FindAuthorBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // 2.d. Lấy và phân trang danh sách tác giả kèm theo
        // số lượng bài viết của tác giả đó.
        // Kết quả trả về kiểu IPagedList<AuthorItem>.
        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        // 2.e. Thêm hoặc cập nhật thông tin một tác giả.
        Task AddOrUpdateAuthorAsync(
            Author author,
            CancellationToken cancellationToken = default);

        Task<bool> IsAuthorSlugExistedAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default);

        // 2.f. Tìm danh sách N tác giả có nhiều bài viết nhất.
        // N là tham số đầu vào.
        Task<IList<AuthorItem>> FindListAuthorsMostPostAsync(
            int n,
            CancellationToken cancellationToken = default);
    }
}
